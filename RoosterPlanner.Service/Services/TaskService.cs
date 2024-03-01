using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Service.DataModels;
using Task = RoosterPlanner.Models.Models.Task;

namespace RoosterPlanner.Service.Services
{
    public interface ITaskService
    {
        /// <summary>
        /// Makes a call to the repository layer and requests an task based on an id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskResult<Task>> GetTaskAsync(Guid id);

        /// <summary>
        /// Makes a call to the repository layer and requests all tasks based on a filter.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskListResult<Task>> SearchTasksAsync(TaskFilter filter);

        /// <summary>
        /// Adds a task to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<TaskResult<Task>> CreateTaskAsync(Task task);

        /// <summary>
        /// Updates a task in the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<TaskResult<Task>> UpdateTaskAsync(Task task);
        /// <summary>
        /// Removes a task from the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        Task<TaskResult<Task>> RemoveTaskAsync(Task task);

        /// <summary>
        /// Makes a call to the repository layer and requests all categories.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskListResult<Category>> GetAllCategoriesAsync();

        /// <summary>
        /// Makes a call to the repository layer and requests a category based on an id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskResult<Category>> GetCategoryAsync(Guid categoryId);

        /// <summary>
        /// Adds a category to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<TaskResult<Category>> CreateCategoryAsync(Category category);

        /// <summary>
        /// Updates a category to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<TaskResult<Category>> UpdateCategoryAsync(Category category);

        /// <summary>
        /// Removes a category from the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<TaskResult<Category>> RemoveCategoryAsync(Category category);

        /// <summary>
        /// Removes a projectTask from the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<TaskResult<ProjectTask>> RemoveProjectTaskAsync(ProjectTask resultData);

        /// <summary>
        /// Makes a call to the repository layer and requests a projectTask based on an id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskResult<ProjectTask>> GetProjectTaskAsync(Guid id);

        /// <summary>
        /// Makes a call to the repository layer and requests a projectTask based on an projectId and a taskId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskResult<ProjectTask>> GetProjectTaskAsync(Guid projectId, Guid taskId);

        /// <summary>
        /// Adds a projectTask to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskResult<ProjectTask>> AddTaskToProjectAsync(ProjectTask projectTask);

        /// <summary>
        /// Makes a call to the repository layer and requests all projectTasks based on an projectId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskListResult<ProjectTask>> GetAllProjectTasksAsync(Guid projectId);
    }

    public class TaskService : ITaskService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<TaskService> logger;
        private readonly ITaskRepository taskRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProjectTaskRepository projectTaskRepository;
        private readonly IDocumentService documentService;

        #endregion

        //Constructor
        public TaskService(
            IUnitOfWork unitOfWork,
            ILogger<TaskService> logger,
            IDocumentService documentService)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            taskRepository = unitOfWork.TaskRepository;
            categoryRepository = unitOfWork.CategoryRepository;
            projectTaskRepository = unitOfWork.ProjectTaskRepository;
            this.documentService = documentService;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests an task based on an id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskResult<Task>> GetTaskAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            TaskResult<Task> result = new TaskResult<Task>();
            try
            {
                result.Data = await taskRepository.GetTaskAsync(id);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting task " + id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests all tasks based on a filter.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskListResult<Task>> SearchTasksAsync(TaskFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            TaskListResult<Task> result = TaskListResult<Task>.CreateDefault();
            try
            {
                result.Data = await taskRepository.SearchTasksAsync(filter);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting task with filter: " + filter;
                logger.LogError(ex, result.Message, filter);
                result.Error = ex;
            }

            return result;
        }
        /// <summary>
        /// Adds a task to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TaskResult<Task>> CreateTaskAsync(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            TaskResult<Task> result = new TaskResult<Task>();
            try
            {
                result.Data = taskRepository.Add(task);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error creating task " + task.Id;
                logger.LogError(ex, result.Message, task);
                result.Error = ex;
            }

            return result;
        }
        /// <summary>
        /// Updates a task in the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TaskResult<Task>> UpdateTaskAsync(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            TaskResult<Task> result = new TaskResult<Task>();
            try
            {
                result.Data = taskRepository.Update(task);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
                
                result.Data.Category.Tasks = null;
                foreach (Requirement requirement in result.Data.Requirements)
                {
                    requirement.CertificateType.Requirements = null;
                    requirement.Task.Requirements = null;

                }
               
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating task " + task.Id;
                logger.LogError(ex, result.Message, task);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Removes a task from the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task<TaskResult<Task>> RemoveTaskAsync(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            TaskResult<Task> result = new TaskResult<Task>();
            try
            {
                if (task.Instruction != null)
                    await documentService.DeleteDocumentAsync(task.Instruction);

                result.Data = taskRepository.Remove(task);
                result.Succeeded = await unitOfWork.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error removing task " + task.Id;
                logger.LogError(ex, result.Message, task);
                result.Error = ex;
            }

            return result;
        }


        /// <summary>
        /// Makes a call to the repository layer and requests all categories.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskListResult<Category>> GetAllCategoriesAsync()
        {
            TaskListResult<Category> result = TaskListResult<Category>.CreateDefault();
            try
            {
                result.Data = await categoryRepository.GetAllCategoriesAsync();
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting all categories ";
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }
        /// <summary>
        /// Makes a call to the repository layer and requests a category based on an id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskResult<Category>> GetCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new ArgumentNullException(nameof(categoryId));

            TaskResult<Category> result = new TaskResult<Category>();
            try
            {
                result.Data = await categoryRepository.GetAsync(categoryId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting category " + categoryId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Adds a category to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TaskResult<Category>> CreateCategoryAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            TaskResult<Category> result = new TaskResult<Category>();

            try
            {
                result.Data = categoryRepository.Add(category);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error creating category " + category.Id;
                logger.LogError(ex, result.Message, category);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Updates a category to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TaskResult<Category>> UpdateCategoryAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            TaskResult<Category> result = new TaskResult<Category>();

            try
            {
                result.Data = categoryRepository.Update(category);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating category " + category.Id;
                logger.LogError(ex, result.Message, category);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Removes a category from the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TaskResult<Category>> RemoveCategoryAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            TaskResult<Category> result = new TaskResult<Category>();
            try
            {
                result.Data = unitOfWork.CategoryRepository.Remove(category);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error removing category " + category.Id;
                logger.LogError(ex, result.Message, category);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Removes a projectTask from the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<TaskResult<ProjectTask>> RemoveProjectTaskAsync(ProjectTask projectTask)
        {
            if (projectTask == null)
                throw new ArgumentNullException(nameof(projectTask));

            TaskResult<ProjectTask> result = new TaskResult<ProjectTask>();
            try
            {
                result.Data = projectTaskRepository.Remove(projectTask);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error removing projecttask " + projectTask.Id;
                logger.LogError(ex, result.Message, projectTask);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests a projectTask based on an id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskResult<ProjectTask>> GetProjectTaskAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            TaskResult<ProjectTask> result = new TaskResult<ProjectTask>();
            try
            {
                result.Data = await projectTaskRepository.GetAsync(id);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting projecttask " + id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests a projectTask based on an projectId and a taskId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskResult<ProjectTask>> GetProjectTaskAsync(Guid projectId, Guid taskId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            if (taskId == Guid.Empty)
                throw new ArgumentNullException(nameof(taskId));
            TaskResult<ProjectTask> result = new TaskResult<ProjectTask>();
            try
            {
                result.Data = await projectTaskRepository.GetProjectTaskAsync(projectId, taskId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting projecttask " + projectId + " " + taskId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Adds a projectTask to the database
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskResult<ProjectTask>> AddTaskToProjectAsync(ProjectTask projectTask)
        {
            if (projectTask == null)
                throw new ArgumentNullException(nameof(projectTask));
            TaskResult<ProjectTask> result = new TaskResult<ProjectTask>();
            try
            {
                projectTask.Project = null;
                projectTask.Task = null;
                result.Data = projectTaskRepository.Add(projectTask);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error adding task to project " + projectTask.Id;
                logger.LogError(ex, result.Message, projectTask);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests all projectTasks based on an projectId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskListResult<ProjectTask>> GetAllProjectTasksAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));

            TaskListResult<ProjectTask> result = TaskListResult<ProjectTask>.CreateDefault();
            try
            {
                result.Data = await projectTaskRepository.GetAllFromProjectAsync(projectId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting tasks from project " + projectId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }
    }
}