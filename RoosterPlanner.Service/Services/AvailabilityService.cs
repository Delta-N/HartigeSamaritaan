using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IAvailabilityService
    {
        /// <summary>
        /// Makes a call to the repository layer and adds a availability to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="availability"></param>
        /// <returns></returns>
        Task<TaskResult<Availability>> AddAvailability(Availability availability);

        /// <summary>
        /// Makes a call to the repository layer and requests an update of a availability.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="availability"></param>
        /// <returns></returns>
        Task<TaskResult<Availability>> UpdateAvailability(Availability availability);

        /// <summary>
        /// Makes a call to the repository layer and requests availabilities based on a projectId and a userId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<TaskListResult<Availability>> FindAvailabilitiesAsync(Guid projectId, Guid userId);

        /// <summary>
        /// Makes a call to the repository layer and requests an availability based on a id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskResult<Availability>> GetAvailability(Guid id);

        /// <summary>
        /// Makes a call to the repository layer and requests 'active' availabilities based on a participationId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participationId"></param>
        /// <returns></returns>
        Task<TaskListResult<Availability>> GetActiveAvailabilities(Guid participationId);

        /// <summary>
        /// Makes a call to the repository layer and requests scheduled availabilities based on a participationId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participationId"></param>
        /// <returns></returns>
        Task<TaskListResult<Availability>> GetScheduledAvailabilities(Guid participationId);

        /// <summary>
        /// Makes a call to the repository layer and requests scheduled availabilities based on a projectId and a date.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        Task<TaskListResult<Availability>> GetScheduledAvailabilities(Guid projectId, DateTime dateTime);

        /// <summary>
        /// Makes a call to the repository layer and requests an availability based on a participationId and shiftId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participationId"></param>
        /// <param name="shiftId"></param>
        /// <returns></returns>
        Task<TaskResult<Availability>> GetAvailability(Guid participationId, Guid shiftId);
    }

    public class AvailabilityService : IAvailabilityService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AvailabilityService> logger;
        private readonly IAvailabilityRepository availabilityRepository;

        #endregion

        public AvailabilityService(IUnitOfWork unitOfWork, ILogger<AvailabilityService> logger)
        {
            availabilityRepository = unitOfWork.AvailabilityRepository;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        /// <summary>
        /// Makes a call to the repository layer and adds a availability to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="availability"></param>
        /// <returns></returns>
        public async Task<TaskResult<Availability>> AddAvailability(Availability availability)
        {
            if (availability == null)
                throw new ArgumentNullException(nameof(availability));

            TaskResult<Availability> result = new TaskResult<Availability>();
            try
            {
                result.Data = availabilityRepository.Add(availability);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error adding Availability " + availability.Id;
                logger.LogError(ex, result.Message, availability);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests an update of a availability.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="availability"></param>
        /// <returns></returns>
        public async Task<TaskResult<Availability>> UpdateAvailability(Availability availability)
        {
            if (availability == null)
                throw new ArgumentNullException(nameof(availability));

            TaskResult<Availability> result = new TaskResult<Availability>();
            try
            {
                result.Data = availabilityRepository.Update(availability);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
                result.Data.Participation?.Availabilities.ForEach(a => a.Participation = null);
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating Availability " + availability.Id;
                logger.LogError(ex, result.Message, availability);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests availabilities based on a projectId and a userId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<TaskListResult<Availability>> FindAvailabilitiesAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            TaskListResult<Availability> result = TaskListResult<Availability>.CreateDefault();
            try
            {
                result.Data = await availabilityRepository.FindAvailabilitiesAsync(projectId, userId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error finding Availabilities " + projectId + userId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests an availability based on a id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskResult<Availability>> GetAvailability(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException(nameof(id));
            TaskResult<Availability> result = new TaskResult<Availability>();
            try
            {
                result.Data = await availabilityRepository.GetAsync(id);
                result.Succeeded = result.Data != null;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting Availability with Id " + id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests 'active' availabilities based on a participationId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participationId"></param>
        /// <returns></returns>
        public async Task<TaskListResult<Availability>> GetActiveAvailabilities(Guid participationId)
        {
            if (participationId == Guid.Empty)
                throw new ArgumentNullException(nameof(participationId));

            TaskListResult<Availability> result = TaskListResult<Availability>.CreateDefault();
            try
            {
                result.Data = await availabilityRepository.GetActiveAvailabilities(participationId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error finding active Availabilities " + participationId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests scheduled availabilities based on a participationId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participationId"></param>
        /// <returns></returns>
        public async Task<TaskListResult<Availability>> GetScheduledAvailabilities(Guid participationId)
        {
            if (participationId == Guid.Empty)
                throw new ArgumentNullException(nameof(participationId));

            TaskListResult<Availability> result = TaskListResult<Availability>.CreateDefault();
            try
            {
                result.Data = await availabilityRepository.GetScheduledAvailabilities(participationId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error finding scheduled Availabilities " + participationId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests scheduled availabilities based on a projectId and a date.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public async Task<TaskListResult<Availability>> GetScheduledAvailabilities(Guid projectId, DateTime dateTime)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));

            TaskListResult<Availability> result = TaskListResult<Availability>.CreateDefault();
            try
            {
                result.Data = await availabilityRepository.GetScheduledAvailabilities(projectId, dateTime);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error finding scheduled Availabilities on: " + dateTime;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests an availability based on a participationId and shiftId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participationId"></param>
        /// <param name="shiftId"></param>
        /// <returns></returns>
        public async Task<TaskResult<Availability>> GetAvailability(Guid participationId, Guid shiftId)
        {
            if (participationId == Guid.Empty)
                throw new ArgumentNullException(nameof(participationId));
            if (shiftId == Guid.Empty)
                throw new ArgumentNullException(nameof(shiftId));

            TaskResult<Availability> result = new TaskResult<Availability>();
            try
            {
                result.Data = await availabilityRepository.GetAvailability(participationId, shiftId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting Availability: " + participationId + " " + shiftId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }
    }
}