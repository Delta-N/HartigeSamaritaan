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
using Type = RoosterPlanner.Api.Models.Type;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService taskService;
        private readonly IDocumentService documentService;
        private readonly IProjectService projectService;
        private readonly ILogger<TasksController> logger;

        //Constructor
        public TasksController(ITaskService taskService, IProjectService projectService,
            ILogger<TasksController> logger, IDocumentService documentService)
        {
            this.taskService = taskService;
            this.projectService = projectService;
            this.logger = logger;
            this.documentService = documentService;
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
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null)
                    return NotFound();
                return Ok(TaskViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetTaskAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskViewModel>>> SearchTasksAsync(string name, int offset = 0,
            int pageSize = 20)
        {
            TaskFilter filter = new TaskFilter(offset, pageSize)
            {
                Name = name
            };
            try
            {
                TaskListResult<Task> result = await taskService.SearchTasksAsync(filter);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});

                if (result.Data == null)
                    return Ok(new List<TaskViewModel>());
                List<TaskViewModel> taskVmList = result.Data.Select(TaskViewModel.CreateVm).ToList();
                return Ok(new SearchResultViewModel<TaskViewModel>(filter.TotalItemCount, taskVmList));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SearchTasksAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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
                task.Instruction = null;

                TaskResult<Task> result;
                if (task.Id == Guid.Empty)
                    result = await taskService.CreateTaskAsync(task);
                else
                    return BadRequest("Cannot update existing Task with post method");

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(TaskViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SaveTaskAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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
                oldTask.ProjectTasks = updatedTask.ProjectTasks;
                if (updatedTask.Instruction != null)
                {
                    oldTask.Instruction =
                        (await documentService.GetDocumentAsync(updatedTask.InstructionId ?? Guid.Empty))
                        .Data;
                    oldTask.InstructionId = oldTask.Instruction.Id;
                }

                if (updatedTask.CategoryId != Guid.Empty && oldTask.CategoryId != updatedTask.CategoryId)
                {
                    oldTask.Category = (await taskService.GetCategoryAsync(updatedTask.CategoryId ?? Guid.Empty)).Data;
                    oldTask.CategoryId = oldTask.Category.Id;
                }
                

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldTask.LastEditBy = oid;

                TaskResult<Task> result = await taskService.UpdateTaskAsync(oldTask);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(TaskViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(UpdateTaskAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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
                return !result.Succeeded
                    ? UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = result.Message})
                    : Ok(TaskViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(RemoveTaskAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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
                string message = GetType().Name + "Error in " + nameof(GetAllCategoriesAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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
                string message = GetType().Name + "Error in " + nameof(GetCategoryAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(CategoryViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SaveCategoryAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldCategory.LastEditBy = oid;

                TaskResult<Category> result = await taskService.UpdateCategoryAsync(oldCategory);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(CategoryViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(UpdateCategoryAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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
                return !result.Succeeded
                    ? UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = result.Message})
                    : Ok(CategoryViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(RemoveCategoryAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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
                string message = GetType().Name + "Error in " + nameof(GetProjectTaskAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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
                string message = GetType().Name + "Error in " + nameof(GetAllProjectTasksAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(TaskViewModel.CreateVm(result.Data.Task));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(AddTaskToProjectAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
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

                return !result.Succeeded
                    ? UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = result.Message})
                    : Ok(TaskViewModel.CreateVm(result.Data.Task));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(RemoveTaskFromProjectAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
    }
}