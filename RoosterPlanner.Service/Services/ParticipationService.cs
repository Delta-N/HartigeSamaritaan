using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using Person = RoosterPlanner.Models.Person;

namespace RoosterPlanner.Service
{
    public interface IParticipationService
    {
        Task<TaskResult<Participation>> AddParticipationAsync(Participation participation);
        Task<TaskListResult<Participation>> GetUserParticipationsAsync(Guid personId);
        Task<TaskResult<Participation>> GetParticipationAsync(Guid participationId);
        Task<TaskResult<Participation>> GetParticipationAsync(Guid personId, Guid projectId);
        Task<TaskResult<Participation>> UpdateParticipationAsync(Participation participation);
    }

    public class ParticipationService : IParticipationService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly IParticipationRepository participationRepository;
        private readonly IAzureB2CService azureB2CService;
        private readonly IPersonService personService;
        private readonly ILogger<ParticipationService> logger;

        #endregion

        //Constructor
        public ParticipationService(IUnitOfWork unitOfWork, ILogger<ParticipationService> logger,
            IAzureB2CService azureB2CService, IPersonService personService)
        {
            this.unitOfWork = unitOfWork;
            participationRepository = unitOfWork.ParticipationRepository;
            this.azureB2CService = azureB2CService;
            this.personService = personService;
            this.logger = logger;
        }

        /// <summary>
        ///     Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public async Task<TaskResult<Participation>> AddParticipationAsync(Participation participation)
        {
            if (participation == null)
                throw new ArgumentNullException(nameof(participation));

            TaskResult<Participation> result = new TaskResult<Participation>();
            try
            {
                User user = await azureB2CService.GetUserAsync(participation.PersonId);
                Person person = (await personService.GetPersonAsync(participation.PersonId)).Data;
                if (user == null)
                    throw new RecordNotFoundException("Person: " + participation.PersonId);

                Project project = await unitOfWork.ProjectRepository.GetAsync(participation.ProjectId);
                if (project == null)
                    throw new RecordNotFoundException("Project: " + participation.ProjectId);

                participation.Person = person;
                participation.Project = project; //todo test if add participation still works

                result.Data = participationRepository.Add(participation);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error adding Participation to LocalDB " + participation.Id;
                logger.LogError(ex, result.Message, participation);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        ///     Returns a list of participations that the user is registerd for
        /// </summary>
        /// <returns>Returns a list of participations.</returns>
        public async Task<TaskListResult<Participation>> GetUserParticipationsAsync(Guid personId)
        {
            if (personId == Guid.Empty)
                throw new ArgumentNullException(nameof(personId));

            TaskListResult<Participation> result = TaskListResult<Participation>.CreateDefault();
            try
            {
                result.Data = await participationRepository.GetActiveParticipationsAsync(personId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error finding participations " + personId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Participation>> GetParticipationAsync(Guid participationId)
        {
            if (participationId == Guid.Empty)
                throw new ArgumentNullException(nameof(participationId));

            TaskResult<Participation> result = new TaskResult<Participation>();
            try
            {
                result.Data = await participationRepository.GetAsync(participationId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error finding participation " + participationId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Participation>> GetParticipationAsync(Guid personId, Guid projectId)
        {
            if (personId == Guid.Empty)
                throw new ArgumentNullException(nameof(personId));

            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            TaskResult<Participation> result = new TaskResult<Participation>();
            try
            {
                Participation participation =
                    await participationRepository.GetSpecificParticipationAsync(personId, projectId);
                if (participation != null)
                {
                    result.Data = participation;
                    result.Succeeded = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error finding participation " + personId + " " + projectId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }
        
        public async Task<TaskResult<Participation>> UpdateParticipationAsync(Participation participation)
        {
            if (participation == null)
                throw new ArgumentNullException(nameof(participation));

            TaskResult<Participation> result = new TaskResult<Participation>();
            try
            {
                result.Data = participationRepository.Update(participation);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating participation " + participation.Id;
                logger.LogError(ex, result.Message, participation);
                result.Error = ex;
            }

            return result;
        }
    }
}