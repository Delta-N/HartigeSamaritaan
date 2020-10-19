using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IMapper mapper = null;
        private readonly IPersonService personService = null;
        private readonly IProjectService projectService = null;
        private readonly ILogger logger = null;

        //Constructor
        public PersonsController(IMapper mapper, IPersonService personService, IProjectService projectService, ILogger logger)
        {
            this.mapper = mapper;
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
                TaskResult<Person> result = await this.personService.GetUser(id);
                personVm = null;
                if (result.Succeeded)
                {
                    personVm = new PersonViewModel
                    {
                        Name = result.Data.Name, 
                        Id = result.Data.Oid
                    };
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