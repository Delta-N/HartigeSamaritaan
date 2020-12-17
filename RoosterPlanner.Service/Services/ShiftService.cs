using System;
using System.Collections.Generic;
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
        Task<TaskResult<Shift>> RemoveShiftAsync(Shift shift);
        Task<TaskResult<Shift>> GetShiftAsync(Guid shiftId);
        Task<TaskResult<Shift>> GetShiftWithAvailabilitiesAsync(Guid shiftId);
        Task<TaskResult<Shift>> UpdateShiftAsync(Shift shift);
        Task<TaskListResult<Shift>> CreateShiftsAsync(List<Shift> shifts);
        Task<TaskListResult<Shift>> GetShiftsAsync(Guid projectId);
        Task<TaskListResult<Shift>> GetShiftsAsync(Guid projectId, DateTime date);
        Task<TaskListResult<Shift>> GetShiftsAsync(Guid projectId, Guid userId, DateTime date);
        Task<TaskListResult<Shift>> GetShiftsWithAvailabilitiesAsync(Guid projectId);
        Task<TaskListResult<Shift>> GetShiftsWithAvailabilitiesAsync(Guid projectId, Guid userId);
        
    }

    public class ShiftService : IShiftService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly IShiftRepository shiftRepository;
        private readonly ILogger<ShiftService> logger;

        #endregion

        //Constructor
        public ShiftService(IUnitOfWork unitOfWork, ILogger<ShiftService> logger)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            shiftRepository = unitOfWork.ShiftRepository ?? throw new ArgumentNullException(nameof(shiftRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TaskResult<Shift>> RemoveShiftAsync(Shift shift)
        {
            if (shift == null)
                throw new ArgumentNullException(nameof(shift));
            TaskResult<Shift> result = new TaskResult<Shift>();
            try
            {
                result.Data = shiftRepository.Remove(shift);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error removing shift " + shift.Id;
                logger.LogError(ex,result.Message, shift);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Shift>> GetShiftWithAvailabilitiesAsync(Guid shiftId)
        {
            if (shiftId == Guid.Empty)
                throw new ArgumentNullException(nameof(shiftId));

            TaskResult<Shift> result = new TaskResult<Shift>();
            try
            {
                result.Data = await shiftRepository.GetShiftWithAvailabilitiesAsync(shiftId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting shift " + shiftId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Shift>> UpdateShiftAsync(Shift shift)
        {
            if (shift == null)
                throw new ArgumentNullException(nameof(shift));

            TaskResult<Shift> result = new TaskResult<Shift>();
            try
            {
                result.Data = shiftRepository.Update(shift);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating shift " + shift.Id;
                logger.LogError(ex, result.Message, shift);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskListResult<Shift>> CreateShiftsAsync(List<Shift> shifts)
        {
            if (shifts == null || shifts.Count == 0)
                throw new ArgumentNullException(nameof(shifts));
            TaskListResult<Shift> result = TaskListResult<Shift>.CreateDefault();
            try
            {
                result.Data = await shiftRepository.AddShiftsAsync(shifts);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == shifts.Count;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error creating shifts ";
                logger.LogError(ex, result.Message, shifts);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Shift>> GetShiftAsync(Guid shiftId)
        {
            if (shiftId == Guid.Empty)
                throw new ArgumentNullException(nameof(shiftId));

            TaskResult<Shift> result = new TaskResult<Shift>();
            try
            {
                result.Data = await shiftRepository.GetShiftAsync(shiftId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting shift " + shiftId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskListResult<Shift>> GetShiftsAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            
            TaskListResult<Shift> result = TaskListResult<Shift>.CreateDefault();
            try
            {
                result.Data = await shiftRepository.GetByProjectAsync(projectId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting shifts with projectid " + projectId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskListResult<Shift>> GetShiftsAsync(Guid projectId, DateTime date)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            if (date == null)
                throw new ArgumentNullException(nameof(date));

            TaskListResult<Shift> result = TaskListResult<Shift>.CreateDefault();
            try
            {
                result.Data = await shiftRepository.GetByProjectAndDateWithAvailabilitiesAsync(projectId, date);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting shifts with date " + date;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskListResult<Shift>> GetShiftsAsync(Guid projectId, Guid userId, DateTime date)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));
            if (date == null)
                throw new ArgumentNullException(nameof(date));

            TaskListResult<Shift> result = TaskListResult<Shift>.CreateDefault();
            try
            {
                result.Data = await shiftRepository.GetByProjectUserAndDateAsync(projectId, userId, date);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting shifts with date " + date;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskListResult<Shift>> GetShiftsWithAvailabilitiesAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            
            TaskListResult<Shift> result = TaskListResult<Shift>.CreateDefault();
            try
            {
                result.Data = await shiftRepository.GetByProjectWithAvailabilitiesAsync(projectId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting shifts with projectid " + projectId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskListResult<Shift>> GetShiftsWithAvailabilitiesAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            TaskListResult<Shift> result = TaskListResult<Shift>.CreateDefault();
            try
            {
                result.Data = await shiftRepository.GetByProjectWithAvailabilitiesAsync(projectId,userId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting shifts with projectid " + projectId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

       
    }
}