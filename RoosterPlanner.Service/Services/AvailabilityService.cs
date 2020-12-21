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
        Task<TaskResult<Availability>> AddAvailability(Availability availability);
        Task<TaskResult<Availability>> UpdateAvailability(Availability availability);
        Task<TaskListResult<Availability>> FindAvailabilitiesAsync(Guid projectId, Guid userId);
        Task<TaskResult<Availability>> GetAvailability(Guid id);
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

        public async Task<TaskResult<Availability>> UpdateAvailability(Availability availability)
        {
            if (availability == null)
                throw new ArgumentNullException(nameof(availability));

            TaskResult<Availability> result = new TaskResult<Availability>();
            try
            {
                result.Data = availabilityRepository.Update(availability);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
                result.Data.Participation.Availabilities.ForEach(a => a.Participation = null);
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating Availability " + availability.Id;
                logger.LogError(ex, result.Message, availability);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskListResult<Availability>> FindAvailabilitiesAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            TaskListResult<Availability> result = TaskListResult<Availability>.CreateDefault();
            try
            {
                result.Data = await availabilityRepository.FindAvailabilitiesAsync(projectId,userId);
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
    }
}