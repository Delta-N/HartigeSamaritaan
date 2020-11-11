using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
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
        TaskResult<Task> UpdateTask(Task task);
    }

    public class TaskService : ITaskService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        private readonly ITaskRepository taskRepository;

        #endregion

        //Constructor
        public TaskService(IUnitOfWork unitOfWork, ILogger<TaskService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.taskRepository = unitOfWork.TaskRepository;
        }

        public async Task<TaskResult<Task>> GetTask(Guid id)
        {
            if (id == Guid.Empty)
                return null;
            TaskResult<Task> taskResult = new TaskResult<Task>();
            try
            {
                taskResult.Data = null; //await this.unitOfWork.TaskRepository.GetTask(id);
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
                taskResult.Data = null; //await this.taskRepository.SearchTasksAsync(filter);
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

        public TaskResult<Task> UpdateTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            TaskResult<Task> taskResult = new TaskResult<Task>();
            try
            {
                taskResult.Data = this.taskRepository.Update(task);
                taskResult.Succeeded = (this.unitOfWork.SaveChanges() == 1);
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