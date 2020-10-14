using System;
using System.Net;
using System.Threading.Tasks;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Service.DataModels;
using Task = RoosterPlanner.Models.Models.Task;

namespace RoosterPlanner.Service
{
    public interface ITaskService
    {
        Task<TaskListResult<Task>> GetActiveTasksAsync();

        Task<TaskResult> SetTaskDeleteAsync(Guid id);
    }

    public class TaskService : ITaskService
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork = null;
        private readonly ILogger logger = null;
        #endregion

        //Constructor
        public TaskService(IUnitOfWork unitOfWork, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<TaskListResult<Task>> GetActiveTasksAsync()
        {
            var taskResult = TaskListResult<Task>.CreateDefault();

            try
            {
                taskResult.Data = await unitOfWork.TaskRepository.GetActiveTasksAsync();
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
            var result = new TaskResult { StatusCode = HttpStatusCode.NoContent };
            if (id != Guid.Empty)
            {
                var task = await unitOfWork.TaskRepository.FindAsync(id);
                task.DeletedDateTime = DateTime.UtcNow;
                unitOfWork.TaskRepository.Update(task);

                result.Succeeded = unitOfWork.SaveChanges() == 1;
            }
            return result;
        }
    }
}
