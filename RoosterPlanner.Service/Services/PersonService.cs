using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using Person = RoosterPlanner.Models.Person;

namespace RoosterPlanner.Service
{
    public interface IPersonService
    {
        Task<TaskResult<User>> GetUser(Guid id);
        Task<TaskResult<Person>> GetPerson(Guid id);
        Task<TaskListResult<User>> GetB2CMembers(PersonFilter filter);

        Task<TaskResult<User>> UpdatePerson(User user);

        Task<TaskResult<Manager>> RemoveManager(Manager manager);
        Task<TaskResult<Manager>> GetManager(Guid projectId, Guid userId);
        Task<TaskResult<Manager>> MakeManager(Manager manager);
        Task<TaskResult<List<Manager>>> UserManagesOtherProjects(Guid userId);
        Task<TaskListResult<Manager>> GetManagers(Guid projectId);
        Task<TaskListResult<Manager>> GetProjectsManagedBy(Guid userId);
    }

    public class PersonService : IPersonService
    {
        #region Fields

        private readonly IUnitOfWork unitOfWork;
        private readonly IPersonRepository personRepository;
        private readonly IManagerRepository managerRepository;
        private readonly IAzureB2CService azureB2CService;
        private readonly ILogger<PersonService> logger;

        #endregion

        //Constructor
        public PersonService(IUnitOfWork unitOfWork, IAzureB2CService azureB2CService, ILogger<PersonService> logger)
        {
            this.unitOfWork = unitOfWork;
            personRepository = unitOfWork.PersonRepository;
            managerRepository = unitOfWork.ManagerRepository;
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
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
                taskResult.Succeeded = false;
            }

            return taskResult;
        }

        public async Task<TaskResult<Person>> GetPerson(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));
            TaskResult<Person> taskResult = new TaskResult<Person>();
            try
            {
                taskResult.Data = await personRepository.GetAsync(id);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskListResult<User>> GetB2CMembers(PersonFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

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
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                result.Error = ex;
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
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
                taskResult.Succeeded = false;
            }

            return taskResult;
        }
        public async Task<TaskListResult<Manager>> GetManagers(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            TaskListResult<Manager> taskResult = TaskListResult<Manager>.CreateDefault();
            try
            {
                taskResult.Data = await managerRepository.GetAll(projectId);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskListResult<Manager>> GetProjectsManagedBy(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));
            TaskListResult<Manager> taskResult = TaskListResult<Manager>.CreateDefault();
            try
            {
                taskResult.Data = await managerRepository.GetProjectsManagedBy(userId);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Manager>> GetManager(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));

            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            TaskResult<Manager> taskResult = new TaskResult<Manager>();
            try
            {
                taskResult.Data = await managerRepository.GetManager(projectId, userId);
                if (taskResult.Data != null)
                    taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Manager>> RemoveManager(Manager manager)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            TaskResult<Manager> taskResult = new TaskResult<Manager>();
            try
            {
                taskResult.Data = managerRepository.Remove(manager);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<Manager>> MakeManager(Manager manager)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            TaskResult<Manager> taskResult = new TaskResult<Manager>();
            try
            {
                taskResult.Data = managerRepository.Add(manager);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                taskResult.Error = ex;
            }

            return taskResult;
        }

        public async Task<TaskResult<List<Manager>>> UserManagesOtherProjects(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));
            TaskResult<List<Manager>> taskResult = new TaskResult<List<Manager>>();
            try
            {
                taskResult.Data = await managerRepository.UserManagesOtherProjects(userId);
                taskResult.Succeeded = true;
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