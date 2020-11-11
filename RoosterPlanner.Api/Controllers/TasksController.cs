using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Models;
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
                TaskResult<Task> result =  await taskService.GetTask(id); 
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
            if(taskViewModel.Category==null)
                return BadRequest("Category of a Task cannot be empty");
            
            TaskResult<Task> result = new TaskResult<Task>();
            try
            {
                Task task = TaskViewModel.CreateTask(taskViewModel);
                if(task==null)
                    return BadRequest("Unable to convert TaskViewModel to Task");
               
                task.LastEditDate = DateTime.UtcNow;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                task.LastEditBy = oid;

                if (task.Id == Guid.Empty)
                    result = taskService.CreateTask(task);
                else
                {
                    result = taskService.UpdateTask(task);
                }

                if (result.Succeeded)
                    return Ok(TaskViewModel.CreateVm(result.Data));
                return UnprocessableEntity(taskViewModel);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPatch]
        public ActionResult UpdateProject(TaskViewModel taskViewModel)
        {
            if (taskViewModel == null || taskViewModel.Id == Guid.Empty)
                return BadRequest("No valid project received");
            if (string.IsNullOrEmpty(taskViewModel.Name))
                return BadRequest("Name of a Task cannot be empty");
            if(taskViewModel.Category==null)
                return BadRequest("Category of a Task cannot be empty");
            try
            {
                Task oldTask = taskService.GetTask(taskViewModel.Id).Result.Data;
                Task updatedTask = TaskViewModel.CreateTask(taskViewModel);
                if(updatedTask==null)
                    return BadRequest("Unable to convert TaskViewModel to Task");
                oldTask.Category = updatedTask.Category;
                oldTask.Color = updatedTask.Color;
                oldTask.CategoryId = updatedTask.CategoryId;
                oldTask.Description = updatedTask.Description;
                oldTask.Name = updatedTask.Name;
                oldTask.Requirements = updatedTask.Requirements;
                oldTask.Shifts = updatedTask.Shifts; //gevaarlijk goed testen?
                oldTask.DocumentUri = updatedTask.DocumentUri;
                oldTask.ProjectTasks = updatedTask.ProjectTasks;
                oldTask.DeletedDateTime = updatedTask.DeletedDateTime;

                oldTask.LastEditDate = DateTime.UtcNow;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldTask.LastEditBy = oid;
                TaskResult<Task> result = taskService.UpdateTask(oldTask);
                if (!result.Succeeded)
                    return UnprocessableEntity();
                return Ok(TaskViewModel.CreateVm(result.Data));
            } 
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }
    }
}