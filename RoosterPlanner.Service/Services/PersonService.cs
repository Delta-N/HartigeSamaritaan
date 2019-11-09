using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.DataModels.B2C;

namespace RoosterPlanner.Service
{
    public interface IPersonService
    {
        Task<TaskListResult<Person>> GetB2cMembers();

        Task<TaskListResult<Project>> UpdatePersonName(Guid oid, string name);
    }

    public class PersonService : IPersonService
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork = null;
        private readonly IPersonRepository personRepository = null;
        private readonly IAzureB2CService azureB2CService = null;
        private readonly ILogger logger = null;
        #endregion

        //Constructor
        public PersonService(IUnitOfWork unitOfWork, IAzureB2CService azureB2CService, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.personRepository = unitOfWork.PersonRepository;
            this.azureB2CService = azureB2CService;
            this.logger = logger;
        }

        public async Task<TaskListResult<Person>> GetB2cMembers()
        {
            TaskListResult<Person> result = TaskListResult<Person>.CreateDefault();

            TaskResult<List<AppUser>> b2cUsersResult = await this.azureB2CService.GetAllUsersAsync();
            if (b2cUsersResult.Succeeded)
            {
                result.Data = b2cUsersResult.Data.Select(x => new Person { Oid = x.Id, Name = x.DisplayName }).ToList();
                result.Succeeded = true;
            }
            return result;
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
