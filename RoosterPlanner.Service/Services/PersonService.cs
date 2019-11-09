using System;
using System.Threading.Tasks;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IPersonService
    {
        Task<TaskResult<Project>> UpdatePersonName(Guid oid, string name);
    }

    public class PersonService : IPersonService
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork = null;
        private readonly ILogger logger = null;
        #endregion

        private readonly Data.Context.RoosterPlannerContext dataContext = null;

        //Constructor
        public PersonService(IUnitOfWork unitOfWork, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        /// <summary>
        /// Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public async Task<TaskResult<Project>> UpdatePersonName(Guid oid, string name)
        {
            var taskResult = new TaskResult<Project>();

            try
            {
                var person = await unitOfWork.PersonRepository.GetPersonByOidAsync(oid);
                if (person == null)
                {
                    //TODO: change exception
                    throw new Exception("Who Are You?");
                }

                person.Name = name;

                taskResult.Succeeded = (await unitOfWork.SaveChangesAsync() == 1);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het updaten van een persoon.");
                taskResult.Error = ex;
            }
            return taskResult;
        }
    }
}
