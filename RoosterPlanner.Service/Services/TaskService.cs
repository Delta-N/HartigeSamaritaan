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
        TaskResult<Task> CreateTask(Task task);
        Task<TaskResult<Task>> UpdateTask(Task task);
        Task<TaskResult<Task>> RemoveTask(Task task);
        Task<TaskListResult<Category>> GetAllCategories();
        Task<TaskResult<Category>> GetCategory(Guid categoryId);
        TaskResult<Category> CreateCategory(Category category);
        TaskResult<Category> UpdateCategory(Category category);
        Task<TaskResult<Category>> RemoveCategory(Category category);
    }

    public class TaskService : ITaskService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly ITaskRepository taskRepository;
        private readonly ICategoryRepository categoryRepository;

        #endregion

        //Constructor
        public TaskService(IUnitOfWork unitOfWork, ILogger<TaskService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.taskRepository = unitOfWork.TaskRepository;
            this.categoryRepository = unitOfWork.CategoryRepository;
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
                throw new ArgumentNullException("filter");
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

        public TaskResult<Task> CreateTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            TaskResult<Task> taskResult = new TaskResult<Task>();
            try
            {
                taskResult.Data = this.taskRepository.Add(task);
                taskResult.Succeeded = (this.unitOfWork.SaveChanges() == 1);
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
                throw new ArgumentNullException("task");

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
                throw new ArgumentNullException("categoryId");

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

        public TaskResult<Category> CreateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");
            TaskResult<Category> taskResult = new TaskResult<Category>();

            try
            {
                taskResult.Data = this.categoryRepository.Add(category);
                taskResult.Succeeded = (this.unitOfWork.SaveChanges() == 1);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public TaskResult<Category> UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");
            TaskResult<Category> taskResult = new TaskResult<Category>();

            try
            {
                taskResult.Data = this.categoryRepository.Update(category);
                taskResult.Succeeded = (this.unitOfWork.SaveChanges() == 1);
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
            }catch (Exception ex)
            {
                logger.Log(LogLevel.Error,ex.ToString());
                result.Error = ex;
                result.Succeeded = false;
            }

            return result;
        }
    }
}