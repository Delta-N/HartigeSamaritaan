using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IRequirementService
    {
        Task<TaskResult<Requirement>> GetRequirementAsync(Guid id);
        Task<TaskResult<Requirement>> GetRequirementAsync(Guid taskId, Guid certificateTypeId);
        Task<TaskResult<Requirement>> CreateRequirement(Requirement requirement);
        Task<TaskResult<Requirement>> UpdateRequirementAsync(Requirement requirement);
        Task<TaskResult<Requirement>> RemoveCertificateAsync(Requirement requirement);
    }

    public class RequirementService : IRequirementService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AvailabilityService> logger;
        private readonly IRequirementRepository requirementRepository;

        #endregion

        public RequirementService(IUnitOfWork unitOfWork, ILogger<AvailabilityService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.requirementRepository = unitOfWork.RequirementRepository;
        }

        public async Task<TaskResult<Requirement>> GetRequirementAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            TaskResult<Requirement> result = new TaskResult<Requirement>();
            try
            {
                result.Data = await requirementRepository.GetRequirementAsync(id);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting Requirement " + id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Requirement>> GetRequirementAsync(Guid taskId, Guid certificateTypeId)
        {
            if (taskId == Guid.Empty)
                throw new ArgumentNullException(nameof(taskId));
            if (certificateTypeId == Guid.Empty)
                throw new ArgumentNullException(nameof(certificateTypeId));
            TaskResult<Requirement> result = new TaskResult<Requirement>();
            try
            {
                result.Data = await requirementRepository.GetRequirementAsync(taskId, certificateTypeId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting Requirement " + taskId + " " + certificateTypeId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Requirement>> CreateRequirement(Requirement requirement)
        {
            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));

            TaskResult<Requirement> result = new TaskResult<Requirement>();

            try
            {
                result.Data = requirementRepository.Add(requirement);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error creating Requirement " + requirement.Id;
                logger.LogError(ex, result.Message, requirement);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Requirement>> UpdateRequirementAsync(Requirement requirement)
        {
            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));

            TaskResult<Requirement> result = new TaskResult<Requirement>();

            try
            {
                result.Data = requirementRepository.Update(requirement);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating Requirement " + requirement.Id;
                logger.LogError(ex, result.Message, requirement);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Requirement>> RemoveCertificateAsync(Requirement requirement)
        {
            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));

            TaskResult<Requirement> result = new TaskResult<Requirement>();
            try
            {
                result.Data = unitOfWork.RequirementRepository.Remove(requirement);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error removing Requirement " + requirement.Id;
                logger.LogError(ex, result.Message, requirement);
                result.Error = ex;
            }

            return result;
        }
    }
}