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
        private readonly IProjectService projectService;
        private readonly ILogger<TasksController> logger;

        //Constructor
        public TasksController(ITaskService taskService, IProjectService projectService,
            ILogger<TasksController> logger)
        {
            this.taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            this.projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskViewModel>> GetTaskAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<Task> result = await taskService.GetTaskAsync(id);
                if (!result.Succeeded)
                    return UnprocessableEntity("Cannot get task");
                if (result.Data == null)
                    return NotFound();
                return Ok(TaskViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskViewModel>>> SearchAsync(string name, int offset = 0, int pageSize = 20)
        {
            TaskFilter filter = new TaskFilter(offset, pageSize)
            {
                Name = name
            };
            try
            {
                TaskListResult<Task> result = await taskService.SearchTasksAsync(filter);
                if (!result.Succeeded)
                    return UnprocessableEntity(result.Message);
                Request.HttpContext.Response.Headers.Add("totalCount", filter.TotalItemCount.ToString());
                if (result.Data == null)
                    return Ok(new List<TaskViewModel>());
                List<TaskViewModel> taskVmList = result.Data.Select(TaskViewModel.CreateVm).ToList();
                return Ok(taskVmList);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        //alleen een bestuurslid kan taken aanmaken of wijzigen
        [Authorize(Policy = "Boardmember")]
        [HttpPost]
        public async Task<ActionResult<TaskViewModel>> SaveTaskAsync(TaskViewModel taskViewModel)
        {
            if (taskViewModel == null)
                return BadRequest("No valid task received");
            if (string.IsNullOrEmpty(taskViewModel.Name))
                return BadRequest("Name of a Task cannot be empty");
            if (taskViewModel.Category == null)
                return BadRequest("Category of a Task cannot be empty");

            try
            {
                Task task = TaskViewModel.CreateTask(taskViewModel);
                if (task == null)
                    return BadRequest("Unable to convert TaskViewModel to Task");

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                task.LastEditBy = oid;

                task.Category = null;

                TaskResult<Task> result;
                if (task.Id == Guid.Empty)
                    result = await taskService.CreateTaskAsync(task);
                else
                    return BadRequest("Cannot update existing Task with post method");

                if (result.Succeeded)
                    return Ok(TaskViewModel.CreateVm(result.Data));
                return UnprocessableEntity(taskViewModel);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPut]
        public async Task<ActionResult<TaskViewModel>> UpdateTaskAsync(TaskViewModel taskViewModel)
        {
            if (taskViewModel == null || taskViewModel.Id == Guid.Empty)
                return BadRequest("No valid project received");
            if (string.IsNullOrEmpty(taskViewModel.Name))
                return BadRequest("Name of a Task cannot be empty");
            if (taskViewModel.Category == null)
                return BadRequest("Category of a Task cannot be empty");
            try
            {
                Task oldTask = (await taskService.GetTaskAsync(taskViewModel.Id)).Data;
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

                if (updatedTask.CategoryId != Guid.Empty && oldTask.CategoryId != updatedTask.CategoryId)
                {
                    oldTask.Category = (await taskService.GetCategoryAsync(updatedTask.CategoryId ?? Guid.Empty)).Data;
                    oldTask.CategoryId = oldTask.Category.Id;
                }

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldTask.LastEditBy = oid;

                TaskResult<Task> result = await taskService.UpdateTaskAsync(oldTask);
                if (!result.Succeeded)
                    return UnprocessableEntity("Cannot update task");
                return Ok(TaskViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskViewModel>> RemoveTaskAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                Task task = (await taskService.GetTaskAsync(id)).Data;
                if (task == null)
                    return NotFound("Task not found");
                TaskResult<Task> result = await taskService.RemoveTaskAsync(task);
                return result.Succeeded ? Ok(TaskViewModel.CreateVm(result.Data)) : Problem();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<List<CategoryViewModel>>> GetAllCategoriesAsync()
        {
            try
            {
                TaskListResult<Category> result = await taskService.GetAllCategoriesAsync();

                if (!result.Succeeded)
                    if (result.Data == null)
                        return Ok(new List<CategoryViewModel>());
                List<CategoryViewModel> categoryViewmodels = result.Data
                    .Select(CategoryViewModel.CreateVm)
                    .ToList();

                return Ok(categoryViewmodels);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpGet("GetCategory/{categoryId}")]
        public async Task<ActionResult<CategoryViewModel>> GetCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<Category> result = await taskService.GetCategoryAsync(categoryId);

                if (!result.Succeeded)
                    if (result.Data == null)
                        return NotFound();
                CategoryViewModel categoryViewModel = CategoryViewModel.CreateVm(result.Data);
                return Ok(categoryViewModel);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPost("SaveCategory")]
        public async Task<ActionResult<CategoryViewModel>> SaveCategoryAsync(CategoryViewModel categoryViewModel)
        {
            if (categoryViewModel == null)
                return BadRequest("No valid task received");
            if (string.IsNullOrEmpty(categoryViewModel.Name))
                return BadRequest("Name of a Category cannot be empty");

            try
            {
                Category category = CategoryViewModel.CreateCategory(categoryViewModel);
                if (category == null)
                    return BadRequest("Unable to convert CategoryViewModel to Category");

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                category.LastEditBy = oid;

                TaskResult<Category> result;
                if (category.Id == Guid.Empty)
                    result = await taskService.CreateCategoryAsync(category);
                else
                    return BadRequest("Cannot update existing category with post method");

                if (result.Succeeded)
                    return Ok(CategoryViewModel.CreateVm(result.Data));
                return UnprocessableEntity("Cannot save category");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<CategoryViewModel>> UpdateCategoryAsync(CategoryViewModel categoryViewModel)
        {
            if (categoryViewModel == null || categoryViewModel.Id == Guid.Empty || categoryViewModel.Name == null)
                return BadRequest("No valid category received");
            try
            {
                Category oldCategory = (await taskService.GetCategoryAsync(categoryViewModel.Id)).Data;
                if (oldCategory == null)
                    return NotFound("Category not found");
                Category updatedCategory = CategoryViewModel.CreateCategory(categoryViewModel);
                if (updatedCategory == null)
                    return BadRequest("Unable to convert CategoryViewModel to Category");
                oldCategory.Code = updatedCategory.Code;
                oldCategory.Name = updatedCategory.Name;
                oldCategory.UrlPdf = updatedCategory.UrlPdf;

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldCategory.LastEditBy = oid;

                TaskResult<Category> result = await taskService.UpdateCategoryAsync(oldCategory);
                if (!result.Succeeded)
                    return UnprocessableEntity("Cannot update category");
                return Ok(CategoryViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult<CategoryViewModel>> RemoveCategoryAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                Category category = (await taskService.GetCategoryAsync(id)).Data;
                if (category == null)
                    return NotFound("Category not found");
                TaskResult<Category> result = await taskService.RemoveCategoryAsync(category);
                return result.Succeeded ? Ok(CategoryViewModel.CreateVm(result.Data)) : Problem();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpGet("GetProjectTask/{id}")]
        public async Task<ActionResult<ProjectTaskViewModel>> GetProjectTaskAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<ProjectTask> result = await taskService.GetProjectTaskAsync(id);

                if (!result.Succeeded)
                    if (result.Data == null)
                        return NotFound();
                ProjectTaskViewModel projectTaskViewModel = ProjectTaskViewModel.CreateVm(result.Data);
                return Ok(projectTaskViewModel);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpGet("GetAllProjectTasks/{projectId}")]
        public async Task<ActionResult<List<TaskViewModel>>> GetAllProjectTasksAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid id.");

            try
            {
                TaskListResult<ProjectTask> result = await taskService.GetAllProjectTasksAsync(projectId);

                if (!result.Succeeded)
                    if (result.Data == null)
                        return Ok(new List<ProjectTaskViewModel>());

                List<TaskViewModel> taskViewModels =
                    result.Data.Select(projectTask => TaskViewModel.CreateVm(projectTask.Task)).ToList();

                return Ok(taskViewModels);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpPost("AddTaskToProject")]
        public async Task<ActionResult<TaskViewModel>> AddTaskToProjectAsync(ProjectTaskViewModel projectTaskViewModel)
        {
            if (projectTaskViewModel == null)
                return BadRequest("No valid ProjectTask received");
            if (projectTaskViewModel.ProjectId == Guid.Empty || projectTaskViewModel.TaskId == Guid.Empty)
                return BadRequest("Project and/or Task Ids cannot be empty");

            try
            {
                ProjectTask projectTask = ProjectTaskViewModel.CreateProjectTask(projectTaskViewModel);
                if (projectTask == null)
                    return BadRequest("Unable to convert ProjectTaskViewModel to ProjectTask");
                projectTask.Task = (await taskService.GetTaskAsync(projectTask.TaskId)).Data;
                projectTask.Project = (await projectService.GetProjectDetailsAsync(projectTask.ProjectId)).Data;
                if (projectTask.Task == null || projectTask.Project == null)
                    return BadRequest("Unable to add project and or task to projecttask");

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                projectTask.LastEditBy = oid;

                TaskResult<ProjectTask> result;
                if (projectTask.Id == Guid.Empty)
                    result = await taskService.AddTaskToProjectAsync(projectTask);
                else
                    return BadRequest("Cannot update existing ProjectTask with post method");

                if (result.Succeeded)
                    return Ok(TaskViewModel.CreateVm(result.Data.Task));
                return UnprocessableEntity("Cannot add task to project");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpDelete("RemoveTaskFromProject/{projectId}/{taskId}")]
        public async Task<ActionResult<TaskViewModel>> RemoveTaskFromProjectAsync(Guid projectId, Guid taskId)
        {
            if (projectId == Guid.Empty || taskId == Guid.Empty)
                return BadRequest("No valid id received");
            try
            {
                ProjectTask projectTask = (await taskService.GetProjectTaskAsync(projectId, taskId)).Data;
                if (projectTask == null)
                    return NotFound("ProjectTask not found");
                TaskResult<ProjectTask> result = await taskService.RemoveProjectTaskAsync(projectTask);

                return result.Succeeded ? Ok(TaskViewModel.CreateVm(result.Data.Task)) : Problem();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity(ex.Message);
            }
        }
    }
}