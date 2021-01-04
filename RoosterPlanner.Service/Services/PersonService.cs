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
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Service
{
    public interface IPersonService
    {
        Task<TaskResult<User>> GetUserAsync(Guid id);
        Task<TaskResult<Person>> GetPersonAsync(Guid id);
        Task<TaskListResult<User>> GetB2CMembersAsync(PersonFilter filter);
        Task<TaskResult<User>> UpdatePersonAsync(User user);
        Task<TaskResult<Person>> UpdatePersonAsync(Person person);
        Task<TaskResult<Manager>> RemoveManagerAsync(Manager manager);
        Task<TaskResult<Manager>> GetManagerAsync(Guid projectId, Guid userId);
        Task<TaskResult<Manager>> MakeManagerAsync(Manager manager);
        Task<TaskResult<List<Manager>>> UserManagesOtherProjectsAsync(Guid userId);
        Task<TaskListResult<Manager>> GetManagersAsync(Guid projectId);
        Task<TaskListResult<Manager>> GetProjectsManagedByAsync(Guid userId);
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

        private async Task AddPersonToLocalDbAsync(User user)
        {
            if (user?.Id == null)
                throw new ArgumentNullException(nameof(user));

            var result = new TaskResult<Person>();
            try
            {
                var person = await personRepository.GetPersonByOidAsync(Guid.Parse(user.Id));
                if (person == null)
                {
                    person = new Person(Guid.Parse(user.Id))
                        {FirstName = user.GivenName, Oid = Guid.Parse(user.Id), LastName = user.Surname};
                    personRepository.Add(person);
                    await unitOfWork.SaveChangesAsync();
                }
                else if (person.FirstName != user.GivenName || person.LastName != user.Surname)
                {
                    person.FirstName = user.GivenName;
                    person.LastName = user.Surname;
                    person.LastEditDate = DateTime.UtcNow;
                    person.LastEditBy = "SYSTEM";
                    personRepository.Update(person);
                    await unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error adding person to LocalDB " + user.Id;
                logger.LogError(ex, result.Message, user);
                result.Error = ex;
            }
        }

        public async Task<TaskResult<User>> GetUserAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            TaskResult<User> result = new TaskResult<User>();
            try
            {
                User person = await azureB2CService.GetUserAsync(id);
                result.Succeeded = person != null;
                if (!result.Succeeded)
                {
                    result.StatusCode = HttpStatusCode.NotFound;
                    result.Message = ResponseMessage.UserNotFound;
                }
                else
                {
                    result.StatusCode = HttpStatusCode.OK;
                    result.Data = person;
                    await AddPersonToLocalDbAsync(person);
                }
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting user " + id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Person>> GetPersonAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            TaskResult<Person> result = new TaskResult<Person>();
            try
            {
                result.Data = await personRepository.GetPersonByOidAsync(id);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting person " + id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskListResult<User>> GetB2CMembersAsync(PersonFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            TaskListResult<User> result = TaskListResult<User>.CreateDefault();
            try
            {
                var b2CUsersResult = await azureB2CService.GetAllUsersAsync(filter);
                if (b2CUsersResult.Succeeded)
                {
                    result.Data = new List<User>();
                    foreach (var user in b2CUsersResult.Data)
                        result.Data.Add(user);

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
                result.Message = GetType().Name + " - Error getting users " + filter;
                logger.LogError(ex, result.Message, filter);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        ///     Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public async Task<TaskResult<User>> UpdatePersonAsync(User user)
        {
            TaskResult<User> result = new TaskResult<User>();
            try
            {
                TaskResult<User> person = await azureB2CService.UpdateUserAsync(user);
                result.Succeeded = person.Succeeded;
                if (!result.Succeeded)
                {
                    result.Succeeded = false;
                    result.Message = person.Message;
                }
                else
                {
                    result.StatusCode = HttpStatusCode.OK;
                    result.Data = person.Data;
                    await AddPersonToLocalDbAsync(person.Data);
                }
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating user " + user.Id;
                logger.LogError(ex, result.Message, user);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Person>> UpdatePersonAsync(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            TaskResult<Person> result = new TaskResult<Person>();
            try
            {
                result.Data = personRepository.Update(person);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating person " + person.Id;
                logger.LogError(ex, result.Message, person);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskListResult<Manager>> GetManagersAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));

            TaskListResult<Manager> result = TaskListResult<Manager>.CreateDefault();
            try
            {
                result.Data = await managerRepository.GetProjectManagersAsync(projectId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting managers " + projectId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskListResult<Manager>> GetProjectsManagedByAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            TaskListResult<Manager> result = TaskListResult<Manager>.CreateDefault();
            try
            {
                result.Data = await managerRepository.GetProjectsManagedByAsync(userId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting projects managed by " + userId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Manager>> GetManagerAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));

            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            TaskResult<Manager> result = new TaskResult<Manager>();
            try
            {
                result.Data = await managerRepository.GetManagerAsync(projectId, userId);
                if (result.Data != null)
                    result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting manager " + userId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Manager>> RemoveManagerAsync(Manager manager)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            TaskResult<Manager> result = new TaskResult<Manager>();
            try
            {
                result.Data = managerRepository.Remove(manager);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error removing manager " + manager.Id;
                logger.LogError(ex, result.Message, manager);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<Manager>> MakeManagerAsync(Manager manager)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            TaskResult<Manager> result = new TaskResult<Manager>();
            try
            {
                result.Data = managerRepository.Add(manager);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error creating manager " + manager.Id;
                logger.LogError(ex, result.Message, manager);
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<List<Manager>>> UserManagesOtherProjectsAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            TaskResult<List<Manager>> result = new TaskResult<List<Manager>>();
            try
            {
                result.Data = await managerRepository.UserManagesOtherProjectsAsync(userId);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting projects user manages " + userId;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }
    }
}