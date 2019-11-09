using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface ITaskService
    {
        Task<TaskListResult<Models.Task>> GetActiveTasksAsync();

        Task<TaskResult> SetTaskDeleteAsync(Guid id);
    }

    public class TaskService : ITaskService
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork = null;
        private readonly ITaskRepository taskRepository = null;
        private readonly ILogger logger = null;
        #endregion

        //Constructor
        public TaskService(IUnitOfWork unitOfWork, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.taskRepository = unitOfWork.TaskRepository;
            this.logger = logger;
        }

        public async Task<TaskListResult<Models.Task>> GetActiveTasksAsync()
        {
            TaskListResult<Models.Task> taskResult = TaskListResult<Models.Task>.CreateDefault();

            try
            {
                taskResult.Data = await this.taskRepository.GetActiveTasksAsync();
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het ophalen van actieve taken.");
                taskResult.Error = ex;
            }
            return taskResult;
        }

        public async Task<TaskResult> SetTaskDeleteAsync(Guid id)
        {
            TaskResult result = new TaskResult { StatusCode = HttpStatusCode.NoContent };
            if (id != Guid.Empty)
            {
                Models.Task task = await this.taskRepository.FindAsync(id);
                task.DeletedDateTime = DateTime.UtcNow;
                this.taskRepository.Update(task);

                result.Succeeded = (this.unitOfWork.SaveChanges() == 1);
            }
            return result;
        }
    }
}
