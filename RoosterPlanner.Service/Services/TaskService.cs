using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface ITaskService
    {
        Task<TaskListResult<Models.Task>> GetActiveTasksAsync();
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
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het ophalen van actieve taken.");
                taskResult.Error = ex;
            }
            return taskResult;
        }
    }
}
