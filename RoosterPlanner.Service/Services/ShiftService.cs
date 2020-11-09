﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IShiftService
    {
        Task<TaskListResult<Shift>> GetActiveShiftsForProjectAsync(Guid projectId);
    }

    public class ShiftService : IShiftService
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger logger;
        #endregion

        //Constructor
        public ShiftService(IUnitOfWork unitOfWork, ILogger<ShiftService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<TaskListResult<Shift>> GetActiveShiftsForProjectAsync(Guid projectId)
        {
            TaskListResult<Shift> taskResult = TaskListResult<Shift>.CreateDefault();

            try
            {
                taskResult.Data = await unitOfWork.ShiftRepository.GetActiveShiftsForProjectAsync(projectId);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,ex.ToString());
                taskResult.Error = ex;
            }
            return taskResult;
        }
    }
}
