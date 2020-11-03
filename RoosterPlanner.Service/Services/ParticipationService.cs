﻿using System;
using System.Threading.Tasks;
using Microsoft.Graph;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IParticipationService
    {
        Task<TaskResult<Participation>> AddParticipationAsync(Participation participation);
        Task<TaskListResult<Participation>> GetUserParticipations(Guid personGuid);
        TaskResult<Participation> GetParticipation(Guid participationId);
        Task<TaskResult<Participation>> GetParticipation(Guid personGuid, Guid projectGuid);
        Task<TaskResult<Participation>> RemoveParticipation(Participation participation);
        Task<TaskResult<Participation>> UpdateParticipation(Participation participation);
    }

    public class ParticipationService : IParticipationService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly IParticipationRepository participationRepository;
        private readonly IAzureB2CService azureB2CService;
        private readonly ILogger logger;

        #endregion

        //private readonly Data.Context.RoosterPlannerContext dataContext = null;

        //Constructor
        public ParticipationService(IUnitOfWork unitOfWork, ILogger logger, IAzureB2CService azureB2CService)
        {
            this.unitOfWork = unitOfWork;
            participationRepository = unitOfWork.ParticipationRepository;
            this.azureB2CService = azureB2CService;
            this.logger = logger;
        }

        /// <summary>
        /// Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public async Task<TaskResult<Participation>> AddParticipationAsync(Participation participation)
        {
            var taskResult = new TaskResult<Participation>();

            try
            {
                User person = await azureB2CService.GetUserAsync(participation.PersonId);
                if (person == null)
                {
                    throw new Exception("Who Are You?");
                }

                var project = await unitOfWork.ProjectRepository.GetAsync(participation.ProjectId);
                if (project == null)
                {
                    throw new Exception("Wait? What project?");
                }

                participation.Person = null;
                participation.Project = null;

                taskResult.Data = unitOfWork.ParticipationRepository.Add(participation);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error adding participation");
                taskResult.Error = ex;
            }

            return taskResult;
        }

        /// <summary>
        /// Returns a list of participations that the user is registerd for
        /// </summary>
        /// <returns>Returns a list of participations.</returns>
        public async Task<TaskListResult<Participation>> GetUserParticipations(Guid personGuid)
        {
            TaskListResult<Participation> result = TaskListResult<Participation>.CreateDefault();
            try
            {
                result.Data = await participationRepository.GetActiveParticipationsAsync(personGuid);
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                logger.Error(e, "Error loading participations");
                result.Error = e;
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
            catch (Exception e)
            {
                logger.Error(e, "Error loading participations");
                result.Error = e;
                result.Succeeded = false;
            }

            return result;
        }

        public async Task<TaskResult<Participation>> GetParticipation(Guid personGuid, Guid projectGuid)
        {
            TaskResult<Participation> result = new TaskResult<Participation>();
            try
            {
                Participation x = await participationRepository.GetSpecificParticipation(personGuid, projectGuid);
                if (x != null)
                {
                    result.Data = x;
                    result.Succeeded = true;
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Error loading participations");
                result.Error = e;
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
            catch (Exception e)
            {
                logger.Error(e, "Error loading participations");
                result.Error = e;
                result.Succeeded = false;
            }

            return result;
        }

        public async Task<TaskResult<Participation>> UpdateParticipation(Participation participation)
        {
            var taskResult = new TaskResult<Participation>();

            try
            {
                taskResult.Data = unitOfWork.ParticipationRepository.Update(participation);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error adding participation");
                taskResult.Error = ex;
            }

            return taskResult;
        }
    }
}