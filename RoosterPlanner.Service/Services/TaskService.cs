using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service.DataModels;
using Task = RoosterPlanner.Models.Task;

namespace RoosterPlanner.Service
{
    public interface ITaskService
    {
        Task<TaskResult<Task>> GetTask(Guid id);
        Task<TaskListResult<Task>> SearchTasksAsync(TaskFilter filter);
        Task<TaskResult<Task>> CreateTask(Task task);
        Task<TaskResult<Task>> UpdateTask(Task task);
        Task<TaskResult<Task>> RemoveTask(Task task);
        Task<TaskListResult<Category>> GetAllCategories();
        Task<TaskResult<Category>> GetCategory(Guid categoryId);
        Task<TaskResult<Category>> CreateCategory(Category category);
        Task<TaskResult<Category>> UpdateCategory(Category category);
        Task<TaskResult<Category>> RemoveCategory(Category category);
        Task<TaskResult<ProjectTask>> RemoveProjectTask(ProjectTask resultData);
        Task<TaskResult<ProjectTask>> GetProjectTask(Guid id);
        Task<TaskResult<ProjectTask>> GetProjectTask(Guid projectId, Guid taskId);
        Task<TaskResult<ProjectTask>> AddTaskToProject(ProjectTask projectTask);
        Task<TaskListResult<ProjectTask>> GetAllProjectTasks(Guid projectId);
    }

    public class TaskService : ITaskService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly ITaskRepository taskRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProjectTaskRepository projectTaskRepository;

        #endregion

        //Constructor
        public TaskService(IUnitOfWork unitOfWork, ILogger<TaskService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.taskRepository = unitOfWork.TaskRepository;
            this.categoryRepository = unitOfWork.CategoryRepository;
            this.projectTaskRepository = unitOfWork.ProjectTaskRepository;
        }

        public async Task<TaskResult<Task>> GetTask(Guid id)
        {
            if (id == Guid.Empty)
                return null;
            TaskResult<Task> taskResult = new TaskResult<Task>();
            try
            {
                taskResult.Data = await this.unitOfWork.TaskRepository.GetTask(id);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskListResult<Task>> SearchTasksAsync(TaskFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            TaskListResult<Task> taskResult = TaskListResult<Task>.CreateDefault();
            try
            {
                taskResult.Data = await this.taskRepository.SearchTasksAsync(filter);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Task>> CreateTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            TaskResult<Task> taskResult = new TaskResult<Task>();
            try
            {
                taskResult.Data = this.taskRepository.Add(task);
                taskResult.Succeeded = await this.unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Task>> UpdateTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            TaskResult<Task> taskResult = new TaskResult<Task>();
            try
            {
                taskResult.Data = unitOfWork.TaskRepository.Update(task);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Task>> RemoveTask(Task task)
        {
            TaskResult<Task> result = new TaskResult<Task>();
            try
            {
                result.Data = taskRepository.Remove(task);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                result.Error = ex;
                result.Succeeded = false;
            }

            return result;
        }

        public async Task<TaskListResult<Category>> GetAllCategories()
        {
            TaskListResult<Category> taskResult = TaskListResult<Category>.CreateDefault();
            try
            {
                taskResult.Data = await this.categoryRepository.GetAll();
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Category>> GetCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new ArgumentNullException(nameof(categoryId));

            TaskResult<Category> taskResult = new TaskResult<Category>();
            try
            {
                taskResult.Data = await this.categoryRepository.GetAsync(categoryId);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Category>> CreateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            TaskResult<Category> taskResult = new TaskResult<Category>();

            try
            {
                taskResult.Data = this.categoryRepository.Add(category);
                taskResult.Succeeded = await this.unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Category>> UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            TaskResult<Category> taskResult = new TaskResult<Category>();

            try
            {
                taskResult.Data = this.categoryRepository.Update(category);
                taskResult.Succeeded = await this.unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Category>> RemoveCategory(Category category)
        {
            TaskResult<Category> result = new TaskResult<Category>();
            try
            {
                result.Data = unitOfWork.CategoryRepository.Remove(category);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                result.Error = ex;
                result.Succeeded = false;
            }

            return result;
        }

        public async Task<TaskResult<ProjectTask>> RemoveProjectTask(ProjectTask projectTask)
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
                logger.Log(LogLevel.Error, ex.ToString());
                result.Error = ex;
                result.Succeeded = false;
            }

            return result;
        }

        public async Task<TaskResult<ProjectTask>> GetProjectTask(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            TaskResult<ProjectTask> taskResult = new TaskResult<ProjectTask>();
            try
            {
                taskResult.Data = await this.projectTaskRepository.GetAsync(id);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<ProjectTask>> GetProjectTask(Guid projectId, Guid taskId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException("projectId");
            if (taskId == Guid.Empty)
                throw new ArgumentNullException("taskId");
            TaskResult<ProjectTask> taskResult = new TaskResult<ProjectTask>();
            try
            {
                taskResult.Data = await this.projectTaskRepository.GetProjectTask(projectId, taskId);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<ProjectTask>> AddTaskToProject(ProjectTask projectTask)
        {
            if (projectTask == null)
                throw new ArgumentNullException(nameof(projectTask));
            TaskResult<ProjectTask> result = new TaskResult<ProjectTask>();
            try
            {
                result.Data = projectTaskRepository.Add(projectTask);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                result.Error = ex;
                result.Succeeded = false;
            }

            return result;
        }

        public async Task<TaskListResult<ProjectTask>> GetAllProjectTasks(Guid projectId)
        {
            TaskListResult<ProjectTask> taskResult = TaskListResult<ProjectTask>.CreateDefault();
            try
            {
                taskResult.Data = await this.projectTaskRepository.GetAllFromProject(projectId);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }
    }
}