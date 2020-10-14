using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IParticipationService
    {
        Task<TaskResult<Participation>> AddParticipationAsync(Guid Oid, Guid projectId);
    }

    public class ParticipationService : IParticipationService
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork = null;
        private readonly ILogger logger = null;
        #endregion

        private readonly Data.Context.RoosterPlannerContext dataContext = null;

        //Constructor
        public ParticipationService(IUnitOfWork unitOfWork, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        /// <summary>
        /// Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public async Task<TaskResult<Participation>> AddParticipationAsync(Guid Oid, Guid projectId)
        {
            var taskResult = new TaskResult<Participation>();
            
            try
            {
                var person = await unitOfWork.PersonRepository.GetPersonByOidAsync(Oid);
                if(person == null)
                {
                    //TODO: change exception
                    throw new Exception("Who Are You?");
                }
                var project = await unitOfWork.ProjectRepository.GetAsync(projectId);
                if (project == null)
                {
                    //TODO: change exception
                    throw new Exception("Wait? What project?");
                }
                var participation = new Participation()
                {
                    ProjectId = projectId,
                    PersonId = person.Id
                };

                taskResult.Data = unitOfWork.ParticipationRepository.Add(participation);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het toevoegen van een participatie.");
                taskResult.Error = ex;
            }
            return taskResult;
        }
    }
}
