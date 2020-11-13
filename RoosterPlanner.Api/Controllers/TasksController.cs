using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using Task = RoosterPlanner.Models.Task;

namespace RoosterPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly ILogger logger;

        //Constructor
        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            this.taskService = taskService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<Task> result = await taskService.GetTask(id);
                if (!result.Succeeded)
                    return UnprocessableEntity();
                if (result.Data == null)
                    return Ok();
                return Ok(TaskViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskViewModel>>> Search(string name, int offset = 0, int pageSize = 20)
        {
            TaskFilter filter = new TaskFilter(offset, pageSize)
            {
                Name = name,
            };
            try
            {
                TaskListResult<Task> result = await taskService.SearchTasksAsync(filter);
                if (!result.Succeeded)
                    return UnprocessableEntity(result.Message);
                Request.HttpContext.Response.Headers.Add("totalCount", filter.TotalItemCount.ToString());
                if (result.Data == null)
                    return Ok();
                List<TaskViewModel> taskVmList = result.Data.Select(TaskViewModel.CreateVm).ToList();
                return Ok(taskVmList);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        //alleen een bestuurslid kan taken aanmaken of wijzigen
        [Authorize(Policy = "Boardmember")]
        [HttpPost]
        public ActionResult Save(TaskViewModel taskViewModel)
        {
            if (taskViewModel == null)
                return BadRequest("No valid task received");
            if (string.IsNullOrEmpty(taskViewModel.Name))
                return BadRequest("Name of a Task cannot be empty");
            if (taskViewModel.Category == null)
                return BadRequest("Category of a Task cannot be empty");

            TaskResult<Task> result;
            try
            {
                Task task = TaskViewModel.CreateTask(taskViewModel);
                if (task == null)
                    return BadRequest("Unable to convert TaskViewModel to Task");

                task.LastEditDate = DateTime.UtcNow;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                task.LastEditBy = oid;

                task.Category = null;

                if (task.Id == Guid.Empty)
                    result = taskService.CreateTask(task);
                else
                    return BadRequest("Cannot update existing Task with post method");

                if (result.Succeeded)
                    return Ok(TaskViewModel.CreateVm(result.Data));
                return UnprocessableEntity(taskViewModel);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPut]
        public ActionResult UpdateTask(TaskViewModel taskViewModel)
        {
            if (taskViewModel == null || taskViewModel.Id == Guid.Empty)
                return BadRequest("No valid project received");
            if (string.IsNullOrEmpty(taskViewModel.Name))
                return BadRequest("Name of a Task cannot be empty");
            if (taskViewModel.Category == null)
                return BadRequest("Category of a Task cannot be empty");
            try
            {
                Task oldTask = taskService.GetTask(taskViewModel.Id).Result.Data;
                if (oldTask == null)
                    return NotFound("Task not found");
                Task updatedTask = TaskViewModel.CreateTask(taskViewModel);
                if (updatedTask == null)
                    return BadRequest("Unable to convert TaskViewModel to Task");

                oldTask.Color = updatedTask.Color;
                oldTask.Description = updatedTask.Description;
                oldTask.Name = updatedTask.Name;
                oldTask.Requirements = updatedTask.Requirements;
                oldTask.Shifts = updatedTask.Shifts; //gevaarlijk goed testen?
                oldTask.DocumentUri = updatedTask.DocumentUri;
                oldTask.ProjectTasks = updatedTask.ProjectTasks;
                oldTask.DeletedDateTime = updatedTask.DeletedDateTime;

                if (updatedTask.CategoryId != null && oldTask.CategoryId != updatedTask.CategoryId)
                {
                    oldTask.Category = taskService.GetCategory(updatedTask.CategoryId??Guid.Empty).Result.Data;
                    oldTask.CategoryId = oldTask.Category.Id;
                }

                oldTask.LastEditDate = DateTime.UtcNow;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldTask.LastEditBy = oid;

                TaskResult<Task> result = taskService.UpdateTask(oldTask).Result;
                if (!result.Succeeded)
                    return UnprocessableEntity();
                return Ok(TaskViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveTask(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                Task<TaskResult<Task>> task = taskService.GetTask(id);
                if (task == null)
                    return NotFound("Task not found");
                TaskResult<Task> result = await taskService.RemoveTask(task.Result.Data);
                if (result.Succeeded)
                    return Ok(result);
                return Problem();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult> GetAllCategories()
        {
            try
            {
                TaskListResult<Category> result = await taskService.GetAllCategories();

                if (!result.Succeeded)
                    if (result.Data == null)
                        return Ok();
                List<CategoryViewModel> categoryViewmodels = result.Data
                    .Select(CategoryViewModel.CreateVm)
                    .ToList();

                return Ok(categoryViewmodels);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [HttpGet("GetCategory/{categoryId}")]
        public async Task<ActionResult> GetCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<Category> result = await taskService.GetCategory(categoryId);

                if (!result.Succeeded)
                    if (result.Data == null)
                        return Ok();
                CategoryViewModel categoryViewModel = CategoryViewModel.CreateVm(result.Data);
                return Ok(categoryViewModel);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPost("SaveCategory")]
        public ActionResult SaveCategory(CategoryViewModel categoryViewModel)
        {
            if (categoryViewModel == null)
                return BadRequest("No valid task received");
            if (string.IsNullOrEmpty(categoryViewModel.Name))
                return BadRequest("Name of a Category cannot be empty");
            TaskResult<Category> result;

            try
            {
                Category category = CategoryViewModel.CreateCategory(categoryViewModel);
                if (category == null)
                    return BadRequest("Unable to convert CategoryViewModel to Category");

                category.LastEditDate = DateTime.UtcNow;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                category.LastEditBy = oid;

                if (category.Id == Guid.Empty)
                    result = taskService.CreateCategory(category);
                else
                    return BadRequest("Cannot update existing category with post method");

                if (result.Succeeded)
                    return Ok(CategoryViewModel.CreateVm(result.Data));
                return UnprocessableEntity();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPut("UpdateCategory")]
        public ActionResult UpdateCategory(CategoryViewModel categoryViewModel)
        {
            if (categoryViewModel == null || categoryViewModel.Id == Guid.Empty || categoryViewModel.Name == null)
                return BadRequest("No valid category received");
            try
            {
                Category oldCategory = taskService.GetCategory(categoryViewModel.Id).Result.Data;
                if (oldCategory == null)
                    return NotFound("Category not found");
                Category updatedCategory = CategoryViewModel.CreateCategory(categoryViewModel);
                if (updatedCategory == null)
                    return BadRequest("Unable to convert CategoryViewModel to Category");
                oldCategory.Code = updatedCategory.Code;
                oldCategory.Name = updatedCategory.Name;
                oldCategory.UrlPdf = updatedCategory.UrlPdf;

                oldCategory.LastEditDate = DateTime.UtcNow;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldCategory.LastEditBy = oid;

                TaskResult<Category> result = taskService.UpdateCategory(oldCategory);
                if (!result.Succeeded)
                    return UnprocessableEntity();
                return Ok(CategoryViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult> RemoveCategory(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                Task<TaskResult<Category>> category = taskService.GetCategory(id);
                if (category == null)
                    return NotFound("Category not found");
                TaskResult<Category> result = await taskService.RemoveCategory(category.Result.Data);
                if (result.Succeeded)
                    return Ok(result);
                return Problem();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }
    }
}