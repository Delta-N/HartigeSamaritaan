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
using RoosterPlanner.Api.Models.Enums;
using RoosterPlanner.Common.Config;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using Extensions = RoosterPlanner.Api.Models.Constants.Extensions;
using Person = RoosterPlanner.Models.Person;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly AzureAuthenticationConfig azureB2CConfig;
        private readonly ILogger logger;
        private readonly IPersonService personService;
        private readonly IProjectService projectService;

        //Constructor
        public PersonsController(IPersonService personService, ILogger<PersonsController> logger,
            IOptions<AzureAuthenticationConfig> azureB2CConfig, IProjectService projectService)
        {
            this.personService = personService;
            this.projectService = projectService;
            this.logger = logger;
            this.azureB2CConfig = azureB2CConfig.Value;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<User> result;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);

                if (id.ToString() == oid || UserHasRole(oid, UserRole.Boardmember))
                    result = await personService.GetUser(id);
                else
                    return Unauthorized();

                if (!result.Succeeded) return UnprocessableEntity();
                if (result.Data == null)
                    return Ok();

                PersonViewModel personVm =
                    PersonViewModel.CreateVmFromUser(result.Data, Extensions.GetInstance(azureB2CConfig));
                return Ok(personVm);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        //Mogen project admins (medewerkercommissie) ook alle gebruikers opvragen?)
        [Authorize(Policy = "Boardmember")]
        [HttpGet]
        public async Task<ActionResult> Get(string email, string firstName, string lastName, string userRole,
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
                TaskListResult<User> result = await personService.GetB2CMembers(filter);

                if (!result.Succeeded) return UnprocessableEntity();
                if (result.Data == null)
                    return Ok();

                for (int i = 0; i < result.Data.Count(); i++)
                {
                    personViewModels.Add(PersonViewModel.CreateVmFromUser(result.Data[i],
                        Extensions.GetInstance(azureB2CConfig)));
                    if (personViewModels.Count == pageSize)
                    {
                        break;
                    }
                }

                return Ok(personViewModels);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser([FromBody] PersonViewModel personViewModel)
        {
            if (personViewModel == null || personViewModel.Id == Guid.Empty)
                return BadRequest("Invalid User");
            try
            {
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                if (oid == null) return BadRequest("Invalid User");

                //only the owner of a profile or a boardmember can update user data
                if (personViewModel.Id.ToString() == oid ||
                    UserHasRole(oid, UserRole.Boardmember))
                {
                    User user = PersonViewModel.CreateUser(personViewModel, Extensions.GetInstance(azureB2CConfig));
                    TaskResult<User> result = await personService.UpdatePerson(user);
                    if (result.Succeeded)
                        return Ok(PersonViewModel.CreateVmFromUser(result.Data,
                            Extensions.GetInstance(azureB2CConfig)));
                }
                else
                {
                    return Unauthorized();
                }

                return BadRequest("Invalid User");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPut("modifyadmin/{oid}/{modifier}")]
        public async Task<ActionResult> ModAdmin(Guid oid, int modifier)
        {
            if (oid == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                //check if user exists
                TaskResult<User> result = await personService.GetUser(oid);

                if (!result.Succeeded)
                    return UnprocessableEntity();

                User user = new User
                {
                    AdditionalData = new Dictionary<string, object>(),
                    Id = oid.ToString()
                };

                if (modifier == 4)
                {
                    //check if user is also a manager
                    TaskResult<List<Manager>> userManagesOtherProjects =
                        await personService.UserManagesOtherProjects(oid);
                    if (userManagesOtherProjects != null && userManagesOtherProjects.Data != null &&
                        userManagesOtherProjects.Data.Count > 0)
                        modifier = 2;
                }

                user.AdditionalData.Add(Extensions.GetInstance(azureB2CConfig).UserRoleExtension, modifier);
                result = await personService.UpdatePerson(user);
                if (result.Succeeded)
                {
                    PersonViewModel personVm =
                        PersonViewModel.CreateVmFromUser(result.Data, Extensions.GetInstance(azureB2CConfig));
                    return Ok(personVm);
                }

                return UnprocessableEntity();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpGet("managers/{projectId}")]
        public async Task<ActionResult> GetProjectManagers(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid id received");
            try
            {
                TaskListResult<Manager> result = await personService.GetManagers(projectId);
                //get users from b2c and add tot DTO
                foreach (Manager manager in result.Data)
                {
                    User temp = personService.GetUser(manager.PersonId).Result.Data;
                    if (temp != null)
                    {
                        PersonViewModel vm =
                            PersonViewModel.CreateVmFromUser(temp, Extensions.GetInstance(azureB2CConfig));
                        if (vm != null)
                        {
                            Person person = PersonViewModel.CreatePerson(vm);
                            manager.Person = person;
                        }
                    }
                }

                if (!result.Succeeded)
                    return UnprocessableEntity();
                if (result.Data == null || result.Data.Count == 0)
                    return Ok();
                List<ManagerViewModel> managerVmList = result.Data.Select(ManagerViewModel.CreateVm).ToList();
                return Ok(managerVmList);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }
        
        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpGet("projectsmanagedby/{userId}")]
        public async Task<ActionResult> GetProjectsManagedBy(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest("No valid id received");
            try
            {
                TaskListResult<Manager> result = await personService.GetProjectsManagedBy(userId);
                
                if (!result.Succeeded)
                    return UnprocessableEntity();
                if (result.Data == null || result.Data.Count == 0)
                    return Ok();
                List<ManagerViewModel> managerVmList = result.Data.Select(ManagerViewModel.CreateVm).ToList();
                return Ok(managerVmList);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }
        

        [Authorize(Policy = "Boardmember")]
        [HttpPost("makemanager/{projectId}/{userId}")]
        public async Task<ActionResult> MakeManager(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                BadRequest("No valid Ids received.");

            try
            {
                Project project = projectService.GetProjectDetails(projectId).Result.Data;
                if (project == null)
                    return BadRequest("Could not find project");
                User user = personService.GetUser(userId).Result.Data;
                if (user == null)
                    return BadRequest("Could not find user");
                Person person = personService.GetPerson(userId).Result.Data;
                if (person == null)
                    return BadRequest("Could not find person in DB");
                Manager manager = personService.GetManager(projectId, userId).Result.Data;
                if (manager != null)
                    return BadRequest("User already manages this project");

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                manager = new Manager()
                {
                    ProjectId = project.Id,
                    Project = project,
                    PersonId = person.Id,
                    Person = person,
                    LastEditDate = DateTime.UtcNow,
                    LastEditBy = oid,
                };

                TaskResult<Manager> result = await personService.MakeManager(manager);
                await ModAdmin(userId, 2); //make user a manager in B2C

                if (!result.Succeeded)
                    return UnprocessableEntity();
                return Ok(PersonViewModel.CreateVmFromUser(user, Extensions.GetInstance(azureB2CConfig)));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpDelete("removemanager/{projectId}/{userId}")]
        public async Task<ActionResult> RemoveManager(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                BadRequest("No valid Ids received.");

            try
            {
                Manager manager = personService.GetManager(projectId, userId).Result.Data;
                if (manager == null)
                    return BadRequest("User is not a manager of this project");

                TaskResult<Manager> result = await personService.RemoveManager(manager);
                TaskResult<List<Manager>> userManagesOtherProjects =
                    await personService.UserManagesOtherProjects(manager.PersonId);

                if (userManagesOtherProjects != null && userManagesOtherProjects.Data != null &&
                    userManagesOtherProjects.Data.Count == 0)
                    await ModAdmin(userId, 4); //remove user as a manager in B2C

                if (!result.Succeeded)
                    return UnprocessableEntity();
                return Ok(PersonViewModel.CreateVmFromPerson(manager.Person));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        private bool UserHasRole(string oid, UserRole userRole)
        {
            Task<ActionResult> currentUserActionResult = Get(Guid.Parse(oid));
            OkObjectResult okObjectResult = (OkObjectResult) currentUserActionResult.Result;
            if (okObjectResult.Value is PersonViewModel currentUser &&
                currentUser.UserRole == userRole.ToString())
                return true;

            return false;
        }
    }
}