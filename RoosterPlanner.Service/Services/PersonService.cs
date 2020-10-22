using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Graph;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.DataModels.B2C;
using RoosterPlanner.Service.Helpers;
using Person = RoosterPlanner.Models.Person;

namespace RoosterPlanner.Service
{
    //todo user bevat meer informatie dan in het person model kan.
    //Nu worden users teruggegeven aan controller waarin vervolgens viewmodels gemaakt worden
    //zodra we gaan editen moet een viewmodel weer omgezet kunnen worden naar een person (create person methode in personviewmodel?)
    public interface IPersonService
    {
        Task<TaskResult<User>> GetUser(Guid id);
        Task<TaskListResult<User>> GetB2cMembers();

        Task<TaskResult<Project>> UpdatePersonName(Guid oid, string name);
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

        public async Task<TaskResult<User>> GetUser(Guid id)
        {
            TaskResult<User> taskResult = new TaskResult<User>();
            try
            {
                User person = await azureB2CService.GetUserAsync(id);
                taskResult.Succeeded = person != null;
                if (taskResult.Succeeded)
                {
                    taskResult.StatusCode = HttpStatusCode.OK;
                    if (person != null) taskResult.Data = person;
                }
                else
                {
                    taskResult.StatusCode = HttpStatusCode.NotFound;
                    taskResult.Message = ResponseMessage.UserNotFound;
                }
            }
            catch (Exception e)
            {
                logger.Error(e, $"Fout bij het ophalen van gebruiker met id {id}");
                taskResult.Error = e;
                taskResult.Succeeded = false;
            }

            return taskResult;
        }

        public async Task<TaskListResult<User>> GetB2cMembers()
        {
            TaskListResult<User> result = TaskListResult<User>.CreateDefault();
            try
            {
                TaskResult<IEnumerable<User>> b2CUsersResult = await this.azureB2CService.GetAllUsersAsync();
                if (b2CUsersResult.Succeeded)
                {
                    result.Data = new List<User>();
                    foreach (User user in b2CUsersResult.Data)
                    {
                        result.Data.Add(user);
                    }
                    result.Succeeded = true;
                }
                else
                {
                    result.StatusCode = b2CUsersResult.StatusCode;
                    result.Message = b2CUsersResult.Message;
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Fout bij het ophalen van alle gebruikers");
                result.Error = e;
                result.Succeeded = false;
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