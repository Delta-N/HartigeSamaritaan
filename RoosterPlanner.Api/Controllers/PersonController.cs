using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IMapper mapper = null;
        private readonly IPersonService personService = null;
        private readonly IProjectService projectService = null;
        private readonly ILogger logger = null;

        //Constructor
        public PersonController(IMapper mapper, IPersonService personService, IProjectService projectService, ILogger logger)
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
                TaskResult<Person> result = null;//await this.personService.Get(id);
                if (result.Succeeded)
                {
                    //projectDetailsVm = this.mapper.Map<ProjectDetailsViewModel>(result.Data);
                }
                return Ok(personVm);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PersonController: Error occured.");
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