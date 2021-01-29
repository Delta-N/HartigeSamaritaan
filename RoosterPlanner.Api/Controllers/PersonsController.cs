using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Api.Models.Constants;
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
        private readonly ILogger<PersonsController> logger;
        private readonly IPersonService personService;
        private readonly IProjectService projectService;
        private readonly IParticipationService participationService;
        private readonly string b2CExtentionApplicationId;

        //Constructor
        public PersonsController(
            IPersonService personService,
            ILogger<PersonsController> logger,
            IProjectService projectService,
            IParticipationService participationService,
            string b2CExtentionApplicationId)
        {
            this.b2CExtentionApplicationId = b2CExtentionApplicationId;
            this.personService = personService;
            this.projectService = projectService;
            this.logger = logger;
            this.participationService = participationService;
        }

        /// <summary>
        /// Makes a request towards the services layer for a person based on a Id.
        /// Also makes a request toward the B2C service for a user.
        /// If the requesting party is not the person being requested, checks proper credentials (Boardmember or Committeemember)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonViewModel>> GetPersonAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<User> userResult;
                TaskResult<Person> personResult;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);

                bool userIsBoardmember = UserHasRole(UserRole.Boardmember,
                    (ClaimsIdentity) HttpContext.User.Identity);
                //check if user != boardmember || != Committeemember
                bool userIsCommitteemember = UserHasRole(UserRole.Committeemember,
                    (ClaimsIdentity) HttpContext.User.Identity);

                if (id.ToString() == oid || userIsBoardmember || userIsCommitteemember)
                {
                    userResult = await personService.GetUserAsync(id);
                    personResult = await personService.GetPersonAsync(id);
                }
                else
                    return Unauthorized();

                if (!userResult.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = userResult.Message});
                if (!personResult.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = personResult.Message});

                if (userResult.Data == null || personResult.Data == null)
                    return NotFound();

                PersonViewModel personVm = PersonViewModel.CreateVmFromUserAndPerson(userResult.Data,
                    personResult.Data, Extensions.GetInstance(b2CExtentionApplicationId));

                if (!userIsBoardmember && !userIsCommitteemember)
                    personVm.StaffRemark = null;

                return Ok(personVm);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetPersonAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Get persons based on properties.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="userRole"></param>
        /// <param name="city"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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
                        Extensions.GetInstance(b2CExtentionApplicationId)));
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

        /// <summary>
        /// Makes a request towards the services layer for all people involved in a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        //todo should be moved to participationController
        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpGet("participants/{projectId}")]
        public async Task<ActionResult<List<PersonViewModel>>> GetParticipants(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid id");
            try
            {
                TaskListResult<Participation> participationResult =
                    await participationService.GetParticipationsAsync(projectId);
                if (!participationResult.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = participationResult.Message});

                List<PersonViewModel> persons = new List<PersonViewModel>();
                foreach (Participation participation in participationResult.Data)
                {
                    TaskResult<User> userResult = await personService.GetUserAsync(participation.PersonId);
                    if (userResult != null)
                        persons.Add(PersonViewModel.CreateVmFromUser(userResult.Data,
                            Extensions.GetInstance(b2CExtentionApplicationId)));
                }

                return Ok(persons);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetParticipants);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer to update a person in Azure B2C.
        /// Only the owner of a profile, Boardmember or Committeemember can update a profile.
        /// </summary>
        /// <param name="personViewModel"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<PersonViewModel>> UpdateUserAsync([FromBody] PersonViewModel personViewModel)
        {
            if (personViewModel == null || personViewModel.Id == Guid.Empty)
                return BadRequest("Invalid User");
            try
            {
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                if (oid == null) return BadRequest("Invalid User");

                //only the owner of a profile or a boardmember or a committeemember can update user data
                if (personViewModel.Id.ToString() != oid &&
                    !UserHasRole(UserRole.Boardmember, (ClaimsIdentity) HttpContext.User.Identity) &&
                    !UserHasRole(UserRole.Committeemember, (ClaimsIdentity) HttpContext.User.Identity))
                    return BadRequest("Invalid User");

                User user = PersonViewModel.CreateUser(personViewModel,
                    Extensions.GetInstance(b2CExtentionApplicationId));
                TaskResult<User> result = await personService.UpdatePersonAsync(user);

                if (!result.Succeeded)
                    return Unauthorized();
                return Ok(PersonViewModel.CreateVmFromUser(result.Data,
                    Extensions.GetInstance(b2CExtentionApplicationId)));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(UpdateUserAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer to update a personsprofile in the (local) database.
        /// </summary>
        /// <param name="personViewModel"></param>
        /// <returns></returns>
        [HttpPut("UpdatePerson")]
        public async Task<ActionResult<PersonViewModel>> UpdatePerson(PersonViewModel personViewModel)
        {
            if (personViewModel == null || personViewModel.Id == Guid.Empty)
                return BadRequest("Invalid Person");
            try
            {
                TaskResult<Person> personResult = await personService.GetPersonAsync(personViewModel.Id);
                if (personResult == null)
                    return NotFound("Person not found in database");
                if (!personResult.Data.RowVersion.SequenceEqual(personViewModel.RowVersion))
                    return BadRequest("Outdated entity received");

                Person person = personResult.Data;

                bool userIsCommitteemember = UserHasRole(UserRole.Committeemember,
                    (ClaimsIdentity) HttpContext.User.Identity);
                bool userIsBoardmember = UserHasRole(UserRole.Boardmember,
                    (ClaimsIdentity) HttpContext.User.Identity);

                person.PushDisabled = personViewModel.PushDisabled;
                person.PersonalRemark = personViewModel.PersonalRemark;
                foreach (Certificate certificate in person.Certificates)
                    certificate.CertificateType = null;

                if (userIsBoardmember || userIsCommitteemember) //staff
                    person.StaffRemark = personViewModel.StaffRemark;

                if (personViewModel.ProfilePicture != null)
                    person.ProfilePictureId = personViewModel.ProfilePicture.Id;

                TaskResult<Person> result = await personService.UpdatePersonAsync(person);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});

                personViewModel.LastEditBy = result.Data.LastEditBy;
                personViewModel.LastEditDate = result.Data.LastEditDate;
                personViewModel.RowVersion = result.Data.RowVersion;
                return Ok(personViewModel);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(UpdatePerson);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer to modify the persons role.
        /// Only administrators are allow to modify a persons role.
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
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

                user.AdditionalData.Add(Extensions.GetInstance(b2CExtentionApplicationId).UserRoleExtension, modifier);
                result = await personService.UpdatePersonAsync(user);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});

                PersonViewModel personVm =
                    PersonViewModel.CreateVmFromUser(result.Data, Extensions.GetInstance(b2CExtentionApplicationId));
                return Ok(personVm);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(ModAdminAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer to get a all managers based on a projectId.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
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
                        PersonViewModel.CreateVmFromUser(temp, Extensions.GetInstance(b2CExtentionApplicationId));
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

        /// <summary>
        /// Makes a request towards the services layer for all manager-entities (DTO for person and project).
        /// Only a Boardmember or Committeemember can make this request.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Makes a request towards the services layer to add a manager entity in the database.
        /// Only Boardmembers can add manager entities.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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

                PersonViewModel viewModel = PersonViewModel.CreateVmFromUserAndPerson(user, person,
                    Extensions.GetInstance(b2CExtentionApplicationId));
                if (viewModel == null)
                    return BadRequest("Unable to create manager");

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
                if (viewModel.UserRole != "Boardmember")
                    await ModAdminAsync(userId, 2); //make user a manager in B2C

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(PersonViewModel.CreateVmFromUser(user, Extensions.GetInstance(b2CExtentionApplicationId)));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(MakeManagerAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
        /// <summary>
        /// Makes a request towards the services layer to remove a manager entity from the database.
        /// Only Boardmembers can make this request.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpDelete("removemanager/{projectId}/{userId}")]
        public async Task<ActionResult<PersonViewModel>> RemoveManagerAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                BadRequest("No valid Ids received.");

            try
            {
                User user = (await personService.GetUserAsync(userId)).Data;
                if (user == null)
                    return BadRequest("Could not find user");

                Manager manager = (await personService.GetManagerAsync(projectId, userId)).Data;
                if (manager == null)
                    return BadRequest("User is not a manager of this project");

                PersonViewModel viewModel = PersonViewModel.CreateVmFromUserAndPerson(user, manager.Person,
                    Extensions.GetInstance(b2CExtentionApplicationId));
                if (viewModel == null)
                    return BadRequest("Unable to create manager");

                manager.Person = null;
                manager.Project = null;

                TaskResult<Manager> result = await personService.RemoveManagerAsync(manager);

                TaskResult<List<Manager>> userManagesOtherProjects =
                    await personService.UserManagesOtherProjectsAsync(manager.PersonId);

                if (userManagesOtherProjects?.Data != null &&
                    userManagesOtherProjects.Data.Count == 0)
                    if (viewModel.UserRole != "Boardmember")
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
        /// <summary>
        /// Check role a user has based on a the claimsidentity.
        /// </summary>
        /// <param name="userRole"></param>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static bool UserHasRole(UserRole userRole, ClaimsIdentity identity)
        {
            int role = Convert.ToInt32(identity.FindFirst("extension_UserRole")?.Value);
            return (int) userRole == role;
        }
    }
}