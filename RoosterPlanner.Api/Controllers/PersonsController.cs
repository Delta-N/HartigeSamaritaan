using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Api.Models.Enums;
using RoosterPlanner.Common;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using Person = RoosterPlanner.Models.Person;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IMapper mapper = null;
        private readonly IPersonService personService = null;
        private readonly IProjectService projectService = null;
        private readonly ILogger logger = null;

        //Constructor
        public PersonsController(IMapper mapper, IPersonService personService, IProjectService projectService,
            ILogger logger)
        {
            this.mapper = mapper;
            this.personService = personService;
            this.projectService = projectService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            PersonViewModel personVm = null;

            try
            {
                TaskResult<User> result = await this.personService.GetUser(id);
                if (result.Succeeded)
                {
                    personVm = PersonViewModel.CreateVm(result.Data);
                }


                return Ok(personVm);
            }
            catch (Exception ex)
            {
                this.Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            PersonViewModel personVm = new PersonViewModel();

            try
            {
                TaskListResult<Person> result = await this.personService.GetB2cMembers();
                if (result.Succeeded)
                {
                    //projectDetailsVm = this.mapper.Map<ProjectDetailsViewModel>(result.Data);
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PersonController: Error occured.");
                this.Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }
    }
}