using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Graph;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;

namespace RoosterPlanner.Service
{
    public interface IPersonService
    {
        Task<TaskResult<User>> GetUser(Guid id);
        Task<TaskListResult<User>> GetB2CMembers(PersonFilter filter);

        Task<TaskResult<User>> UpdatePerson(User user);
    }

    public class PersonService : IPersonService
    {
        //Constructor
        public PersonService(IUnitOfWork unitOfWork, IAzureB2CService azureB2CService, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            personRepository = unitOfWork.PersonRepository;
            this.azureB2CService = azureB2CService;
            this.logger = logger;
        }

        public async Task<TaskResult<User>> GetUser(Guid id)
        {
            var taskResult = new TaskResult<User>();
            try
            {
                var person = await azureB2CService.GetUserAsync(id);
                taskResult.Succeeded = person != null;
                if (taskResult.Succeeded)
                {
                    taskResult.StatusCode = HttpStatusCode.OK;
                    taskResult.Data = person;
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

        public async Task<TaskListResult<User>> GetB2CMembers(PersonFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            var result = TaskListResult<User>.CreateDefault();
            try
            {
                var b2CUsersResult = await azureB2CService.GetAllUsersAsync(filter);
                if (b2CUsersResult.Succeeded)
                {
                    result.Data = new List<User>();
                    foreach (var user in b2CUsersResult.Data) result.Data.Add(user);

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
        ///     Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public async Task<TaskResult<User>> UpdatePerson(User user)
        {
            var taskResult = new TaskResult<User>();
            try
            {
                var person = await azureB2CService.UpdateUserAsync(user);
                taskResult.Succeeded = person.Succeeded;
                if (taskResult.Succeeded)
                {
                    taskResult.StatusCode = HttpStatusCode.OK;
                    taskResult.Data = person.Data;
                }
                else
                {
                    taskResult.Succeeded = false;
                    taskResult.Message = person.Message;
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Fout tijdens het update van User");
                taskResult.Error = e;
                taskResult.Succeeded = false;
            }

            return taskResult;
        }

        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly IPersonRepository personRepository;
        private readonly IAzureB2CService azureB2CService;
        private readonly ILogger logger;

        #endregion
    }
}