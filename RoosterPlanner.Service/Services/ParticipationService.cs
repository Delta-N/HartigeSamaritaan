﻿#nullable enable
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Email;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using Attachment = System.Net.Mail.Attachment;
using Person = RoosterPlanner.Models.Models.Person;

namespace RoosterPlanner.Service.Services
{
    public interface IParticipationService
    {
        /// <summary>
        /// Makes a call to the repository layer and adds a participation to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        Task<TaskResult<Participation>> AddParticipationAsync(Participation participation);

        /// <summary>
        /// Makes a call to the repository layer and requests all participations based on a personId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<TaskListResult<Participation>> GetUserParticipationsAsync(Guid personId);

        /// <summary>
        /// Makes a call to the repository layer and requests a participation based on a participationId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participationId"></param>
        /// <returns></returns>
        Task<TaskResult<Participation>> GetParticipationAsync(Guid participationId);

        /// <summary>
        /// Makes a call to the repository layer and requests a participation based on a personId and a projectId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<TaskResult<Participation>> GetParticipationAsync(Guid personId, Guid projectId);

        /// <summary>
        /// Makes a call to the repository layer and requests all participations based on a projectId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<TaskListResult<Participation>> GetParticipationsAsync(Guid projectId);

        /// <summary>
        /// Makes a call to the repository layer and requests all participations including availabilities based on a projectId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<TaskListResult<Participation>> GetParticipationsWithAvailabilitiesAsync(Guid projectId);

        /// <summary>
        /// Makes a call to the repository layer and requests an update of a participation.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        Task<TaskResult<Participation>> UpdateParticipationAsync(Participation participation);
    }

    public class ParticipationService : IParticipationService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly IParticipationRepository participationRepository;
        private readonly IAzureB2CService azureB2CService;
        private readonly IPersonService personService;
        private readonly IEmailService emailService;
        private readonly ILogger<ParticipationService> logger;

        #endregion

        //Constructor
        public ParticipationService(IUnitOfWork unitOfWork, ILogger<ParticipationService> logger,
            IAzureB2CService azureB2CService, IPersonService personService, IEmailService emailService)
        {
            this.unitOfWork = unitOfWork;
            participationRepository = unitOfWork.ParticipationRepository;
            this.azureB2CService = azureB2CService;
            this.personService = personService;
            this.emailService = emailService;
            this.logger = logger;
        }

        /// <summary>
        /// Makes a call to the repository layer and adds a participation to the database.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        public async Task<TaskResult<Participation>> AddParticipationAsync(Participation participation)
        {
            if (participation == null)
                throw new ArgumentNullException(nameof(participation));

            TaskResult<Participation> result = new();
            try
            {
                User user = await azureB2CService.GetUserAsync(participation.PersonId);
                Person person = (await personService.GetPersonAsync(participation.PersonId)).Data;
                if (user == null || person == null)
                    throw new RecordNotFoundException("Person: " + participation.PersonId);

                Project project = await unitOfWork.ProjectRepository.GetAsync(participation.ProjectId);
                if (project == null)
                    throw new RecordNotFoundException("Project: " + participation.ProjectId);

                participation.Person = null;
                participation.Project = null;

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
        /// Makes a call to the repository layer and requests all participations based on a personId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Makes a call to the repository layer and requests a participation based on a participationId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participationId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Makes a call to the repository layer and requests a participation based on a personId and a projectId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Makes a call to the repository layer and requests all participations based on a projectId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<TaskListResult<Participation>> GetParticipationsAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));

            TaskListResult<Participation> result = TaskListResult<Participation>.CreateDefault();
            try
            {
                result.Data = await participationRepository.GetParticipations(projectId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error finding participations " + projectId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests all participations including availabilities based on a projectId.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<TaskListResult<Participation>> GetParticipationsWithAvailabilitiesAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));

            TaskListResult<Participation> result = TaskListResult<Participation>.CreateDefault();
            try
            {
                result.Data = await participationRepository.GetParticipationsWithAvailabilities(projectId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error finding participations " + projectId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests an update of a participation.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
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
