using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Common;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;


namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonService personService = null;
        private readonly IProjectService projectService = null;
        private readonly ILogger logger = null;

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
            PersonViewModel personVm = new PersonViewModel();

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

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            List<PersonViewModel> personViewModels = new List<PersonViewModel>();

            try
            {
                TaskListResult<User> result = await this.personService.GetB2cMembers();
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
    }
}