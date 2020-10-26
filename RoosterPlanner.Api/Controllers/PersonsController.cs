using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Common;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using Extensions = RoosterPlanner.Api.Models.Constants.Extensions;


namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService personService;
        private readonly IProjectService projectService;
        private readonly ILogger logger;

        //Constructor
        public PersonsController(IPersonService personService, IProjectService projectService,
            ILogger logger)
        {
            this.personService = personService;
            this.projectService = projectService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            PersonViewModel personVm;

            try
            {
                if (id != Guid.Empty)
                {
                    TaskResult<User> result = await this.personService.GetUser(id);
                    if (result.Succeeded)
                    {
                        personVm = PersonViewModel.CreateVm(result.Data);
                        return Ok(personVm);
                    }

                    return UnprocessableEntity();
                }

                return BadRequest("No valid id.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PersonController - Get(Guid id): Error occured.");
                this.Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        //Mogen project admins (medewerkercommissie) ook alle gebruikers opvragen?)
        [Authorize(Policy = "Boardmember")]
        [HttpGet]
        public async Task<ActionResult> Get(string email, string firstName, string lastName, string userRole,
            string city, int offset = 0, int pageSize = 20)
        {
            //TODO aanpassen zodra dat nodig is
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
                TaskListResult<User> result = await this.personService.GetB2CMembers(filter);
                if (result.Succeeded)
                {
                    foreach (User user in result.Data)
                    {
                        personViewModels.Add(PersonViewModel.CreateVm(user));
                    }

                    return Ok(personViewModels);
                }

                return UnprocessableEntity();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PersonController - Get(): Error occured.");
                this.Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPatch("modifyadmin/{oid}/{modifier}")]
        public async Task<ActionResult> Patch(Guid oid, int modifier)
        {
            try
            {
                if (oid != Guid.Empty)
                {
                    //check if user exists
                    TaskResult<User> result = await this.personService.GetUser(oid);
                    if (result.Succeeded)
                    {
                        User user = new User();
                        if (user.AdditionalData == null)
                        {
                            user.AdditionalData = new Dictionary<string, object>();
                        }

                        user.AdditionalData.Add(Extensions.UserRoleExtension, modifier);
                        result = await personService.UpdatePerson(user, Guid.Parse(result.Data.Id));
                        if (result.Succeeded)
                            return Ok(result.Data);
                    }
                    return UnprocessableEntity();
                }
                return BadRequest("No valid id.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PersonController - Patch(): Error occured.");
                this.Response.Headers.Add("message", ex.Message);
            }
            return NoContent();
        }
    }
}