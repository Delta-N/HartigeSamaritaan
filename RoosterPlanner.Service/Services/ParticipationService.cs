using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;

namespace RoosterPlanner.Service
{
    public interface IParticipationService
    {
        Task<TaskResult<Participation>> AddParticipationAsync(Participation participation);
        Task<TaskListResult<Participation>> GetUserParticipations(Guid personId);
        TaskResult<Participation> GetParticipation(Guid participationId);
        Task<TaskResult<Participation>> GetParticipation(Guid personId, Guid projectId);
        Task<TaskResult<Participation>> RemoveParticipation(Participation participation);
        Task<TaskResult<Participation>> UpdateParticipation(Participation participation);
    }

    public class ParticipationService : IParticipationService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly IParticipationRepository participationRepository;
        private readonly IAzureB2CService azureB2CService;
        private readonly ILogger<ParticipationService> logger;

        #endregion

        //Constructor
        public ParticipationService(IUnitOfWork unitOfWork, ILogger<ParticipationService> logger,
            IAzureB2CService azureB2CService)
        {
            this.unitOfWork = unitOfWork;
            participationRepository = unitOfWork.ParticipationRepository;
            this.azureB2CService = azureB2CService;
            this.logger = logger;
        }

        /// <summary>
        ///     Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public async Task<TaskResult<Participation>> AddParticipationAsync(Participation participation)
        {
            TaskResult<Participation> taskResult = new TaskResult<Participation>();

            try
            {
                User person = await azureB2CService.GetUserAsync(participation.PersonId);
                if (person == null) throw new RecordNotFoundException("Person: " + participation.PersonId);

                Project project = await unitOfWork.ProjectRepository.GetAsync(participation.ProjectId);
                if (project == null) throw new RecordNotFoundException("Project: " + participation.ProjectId);

                participation.Person = null;
                participation.Project = null;

                taskResult.Data = unitOfWork.ParticipationRepository.Add(participation);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        /// <summary>
        ///     Returns a list of participations that the user is registerd for
        /// </summary>
        /// <returns>Returns a list of participations.</returns>
        public async Task<TaskListResult<Participation>> GetUserParticipations(Guid personId)
        {
            TaskListResult<Participation> result = TaskListResult<Participation>.CreateDefault();
            try
            {
                result.Data = await participationRepository.GetActiveParticipationsAsync(personId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                result.Error = ex;
                result.Succeeded = false;
            }

            return result;
        }

        public TaskResult<Participation> GetParticipation(Guid participationId)
        {
            TaskResult<Participation> result = new TaskResult<Participation>();
            try
            {
                result.Data = participationRepository.Get(participationId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                result.Error = ex;
                result.Succeeded = false;
            }

            return result;
        }

        public async Task<TaskResult<Participation>> GetParticipation(Guid personId, Guid projectId)
        {
            TaskResult<Participation> result = new TaskResult<Participation>();
            try
            {
                Participation participation =
                    await participationRepository.GetSpecificParticipation(personId, projectId);
                if (participation != null)
                {
                    result.Data = participation;
                    result.Succeeded = true;
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                result.Error = ex;
                result.Succeeded = false;
            }

            return result;
        }

        public async Task<TaskResult<Participation>> RemoveParticipation(Participation participation)
        {
            TaskResult<Participation> result = new TaskResult<Participation>();
            try
            {
                result.Data = unitOfWork.ParticipationRepository.Remove(participation);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                result.Error = ex;
                result.Succeeded = false;
            }

            return result;
        }

        public async Task<TaskResult<Participation>> UpdateParticipation(Participation participation)
        {
            TaskResult<Participation> taskResult = new TaskResult<Participation>();

            try
            {
                taskResult.Data = unitOfWork.ParticipationRepository.Update(participation);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
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