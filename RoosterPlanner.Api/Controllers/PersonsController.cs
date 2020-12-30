using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Api.Models.Constants;
using RoosterPlanner.Common.Config;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Models.Models.Enums;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using Person = RoosterPlanner.Models.Person;
using Type = RoosterPlanner.Api.Models.Type;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly AzureAuthenticationConfig azureB2CConfig;
        private readonly ILogger<PersonsController> logger;
        private readonly IPersonService personService;
        private readonly IProjectService projectService;

        //Constructor
        public PersonsController(
            IPersonService personService,
            IOptions<AzureAuthenticationConfig> azureB2CConfig, 
            ILogger<PersonsController> logger,
            IProjectService projectService)
        {
            this.personService = personService;
            this.projectService = projectService;
            this.logger = logger;
            this.azureB2CConfig = azureB2CConfig.Value;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonViewModel>> GetPersonAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<User> result;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);

                if (id.ToString() == oid || await UserHasRole(oid, UserRole.Boardmember))
                    result = await personService.GetUserAsync(id);
                else
                    return Unauthorized();

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null)
                    return NotFound();

                PersonViewModel personVm =
                    PersonViewModel.CreateVmFromUser(result.Data, Extensions.GetInstance(azureB2CConfig));
                return Ok(personVm);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetPersonAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        //Mogen project admins (medewerkercommissie) ook alle gebruikers opvragen?)
        [Authorize(Policy = "Boardmember")]
        [HttpGet]
        public async Task<ActionResult<PersonViewModel>> GetPersonAsync(string email, string firstName, string lastName,
            string userRole,
            string city, int offset = 0, int pageSize = 20)
        {
            PersonFilter filter = new PersonFilter(offset, pageSize)
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                UserRole = userRole,
                City = city
            };

            List<PersonViewModel> personViewModels = new List<PersonViewModel>();

            try
            {
                TaskListResult<User> result = await personService.GetB2CMembersAsync(filter);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null)
                    return Ok(new List<PersonViewModel>());

                foreach (User user in result.Data)
                {
                    personViewModels.Add(PersonViewModel.CreateVmFromUser(user,
                        Extensions.GetInstance(azureB2CConfig)));
                    if (personViewModels.Count == pageSize)
                    {
                        break;
                    }
                }

                return Ok(new SearchResultViewModel<PersonViewModel>(filter.TotalItemCount, personViewModels));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetPersonAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpPut]
        public async Task<ActionResult<PersonViewModel>> UpdateUserAsync([FromBody] PersonViewModel personViewModel)
        {
            if (personViewModel == null || personViewModel.Id == Guid.Empty)
                return BadRequest("Invalid User");
            try
            {
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                if (oid == null) return BadRequest("Invalid User");

                //only the owner of a profile or a boardmember can update user data
                if (personViewModel.Id.ToString() != oid && !await UserHasRole(oid, UserRole.Boardmember))
                    return BadRequest("Invalid User");

                User user = PersonViewModel.CreateUser(personViewModel, Extensions.GetInstance(azureB2CConfig));
                TaskResult<User> result = await personService.UpdatePersonAsync(user);

                if (!result.Succeeded)
                    return Unauthorized();
                return Ok(PersonViewModel.CreateVmFromUser(result.Data,
                    Extensions.GetInstance(azureB2CConfig)));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(UpdateUserAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPut("modifyadmin/{oid}/{modifier}")]
        public async Task<ActionResult<PersonViewModel>> ModAdminAsync(Guid oid, int modifier)
        {
            if (oid == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                //check if user exists
                TaskResult<User> result = await personService.GetUserAsync(oid);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});

                User user = new User
                {
                    AdditionalData = new Dictionary<string, object>(),
                    Id = oid.ToString()
                };

                if (modifier == 4)
                {
                    //check if user is also a manager
                    TaskResult<List<Manager>> userManagesOtherProjects =
                        await personService.UserManagesOtherProjectsAsync(oid);
                    if (userManagesOtherProjects?.Data != null && userManagesOtherProjects.Data.Count > 0)
                        modifier = 2;
                }

                user.AdditionalData.Add(Extensions.GetInstance(azureB2CConfig).UserRoleExtension, modifier);
                result = await personService.UpdatePersonAsync(user);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});

                PersonViewModel personVm =
                    PersonViewModel.CreateVmFromUser(result.Data, Extensions.GetInstance(azureB2CConfig));
                return Ok(personVm);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(ModAdminAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpGet("managers/{projectId}")]
        public async Task<ActionResult<List<ManagerViewModel>>> GetProjectManagersAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid id received");
            try
            {
                TaskListResult<Manager> result = await personService.GetManagersAsync(projectId);
                //get users from b2c and add tot DTO
                foreach (Manager manager in result.Data)
                {
                    User temp = (await personService.GetUserAsync(manager.PersonId)).Data;
                    if (temp == null)
                        continue;
                    PersonViewModel vm =
                        PersonViewModel.CreateVmFromUser(temp, Extensions.GetInstance(azureB2CConfig));
                    if (vm == null)
                        continue;

                    Person person = PersonViewModel.CreatePerson(vm);
                    manager.Person = person;
                }

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null || result.Data.Count == 0)
                    return Ok(new List<ManagerViewModel>());
                List<ManagerViewModel> managerVmList = result.Data.Select(ManagerViewModel.CreateVm).ToList();
                return Ok(managerVmList);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetProjectManagersAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpGet("projectsmanagedby/{userId}")]
        public async Task<ActionResult<List<ManagerViewModel>>> GetProjectsManagedByAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest("No valid id received");
            try
            {
                TaskListResult<Manager> result = await personService.GetProjectsManagedByAsync(userId);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null || result.Data.Count == 0)
                    return Ok(new List<ManagerViewModel>());
                List<ManagerViewModel> managerVmList = result.Data.Select(ManagerViewModel.CreateVm).ToList();
                return Ok(managerVmList);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetProjectsManagedByAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPost("makemanager/{projectId}/{userId}")]
        public async Task<ActionResult<PersonViewModel>> MakeManagerAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                BadRequest("No valid Ids received.");

            try
            {
                Project project = (await projectService.GetProjectDetailsAsync(projectId)).Data;
                if (project == null)
                    return BadRequest("Could not find project");
                User user = (await personService.GetUserAsync(userId)).Data;
                if (user == null)
                    return BadRequest("Could not find user");
                Person person = (await personService.GetPersonAsync(userId)).Data;
                if (person == null)
                    return BadRequest("Could not find person in DB");
                Manager manager = (await personService.GetManagerAsync(projectId, userId)).Data;
                if (manager != null)
                    return BadRequest("User already manages this project");

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                manager = new Manager
                {
                    ProjectId = project.Id,
                    Project = project,
                    PersonId = person.Id,
                    Person = person,
                    LastEditBy = oid
                };

                TaskResult<Manager> result = await personService.MakeManagerAsync(manager);
                if (!await UserHasRole(person.Oid.ToString(), UserRole.Boardmember))
                    await ModAdminAsync(userId, 2); //make user a manager in B2C

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(PersonViewModel.CreateVmFromUser(user, Extensions.GetInstance(azureB2CConfig)));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(MakeManagerAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpDelete("removemanager/{projectId}/{userId}")]
        public async Task<ActionResult<PersonViewModel>> RemoveManagerAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                BadRequest("No valid Ids received.");

            try
            {
                Manager manager = (await personService.GetManagerAsync(projectId, userId)).Data;
                if (manager == null)
                    return BadRequest("User is not a manager of this project");

                TaskResult<Manager> result = await personService.RemoveManagerAsync(manager);
                TaskResult<List<Manager>> userManagesOtherProjects =
                    await personService.UserManagesOtherProjectsAsync(manager.PersonId);

                if (userManagesOtherProjects?.Data != null &&
                    userManagesOtherProjects.Data.Count == 0)
                    if (!await UserHasRole(manager.PersonId.ToString(), UserRole.Boardmember))
                        await ModAdminAsync(userId, 4); //remove user as a manager in B2C}

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(PersonViewModel.CreateVmFromPerson(manager.Person));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(RemoveManagerAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        private async Task<bool> UserHasRole(string oid, UserRole userRole)
        {
            PersonViewModel personVm = PersonViewModel.CreateVmFromUser(
                (await personService.GetUserAsync(Guid.Parse(oid))).Data,
                Extensions.GetInstance(azureB2CConfig));
            return personVm.UserRole == userRole.ToString();
        }
    }
}