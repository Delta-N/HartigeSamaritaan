using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IShiftService
    {
        Task<TaskResult<Shift>> RemoveShift(Shift shift);
        Task<TaskResult<Shift>> GetShift(Guid shiftId);
        Task<TaskResult<Shift>> UpdateShift(Shift shift);
        Task<TaskListResult<Shift>> CreateShifts(List<Shift> shifts);
        Task<TaskListResult<Shift>> GetShifts(Guid projectId);
    }

    public class ShiftService : IShiftService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly IShiftRepository shiftRepository;
        private readonly ILogger logger;

        #endregion

        //Constructor
        public ShiftService(IUnitOfWork unitOfWork, ILogger<ShiftService> logger)
        {
            this.unitOfWork = unitOfWork;
            shiftRepository = unitOfWork.ShiftRepository;
            this.logger = logger;
        }

        public async Task<TaskResult<Shift>> RemoveShift(Shift shift)
        {
            if (shift == null)
                throw new ArgumentNullException("shift");
            TaskResult<Shift> taskResult = new TaskResult<Shift>();
            try
            {
                taskResult.Data = shiftRepository.Remove(shift);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Shift>> GetShift(Guid shiftId)
        {
            if (shiftId == Guid.Empty)
                return null;
            TaskResult<Shift> taskResult = new TaskResult<Shift>();
            try
            {
                taskResult.Data = await this.unitOfWork.ShiftRepository.GetShift(shiftId);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Shift>> UpdateShift(Shift shift)
        {
            if(shift==null)
                throw new ArgumentNullException("shift");
            
            TaskResult<Shift> taskResult = new TaskResult<Shift>();
            try
            {
                taskResult.Data = unitOfWork.ShiftRepository.Update(shift);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskListResult<Shift>> CreateShifts(List<Shift> shifts)
        {
            if(shifts==null || shifts.Count==0)
                throw new ArgumentNullException("shifts");
            TaskListResult<Shift> taskResult = TaskListResult<Shift>.CreateDefault();
            try
            {
                taskResult.Data = await this.shiftRepository.AddAll(shifts);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync()==shifts.Count();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskListResult<Shift>> GetShifts(Guid projectId)
        {
            if(projectId==Guid.Empty)
                throw new ArgumentNullException("projectId");
            TaskListResult<Shift> taskResult = TaskListResult<Shift>.CreateDefault();
            try
            {
                taskResult.Data = await this.shiftRepository.GetAll(projectId);
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