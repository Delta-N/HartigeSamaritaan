using System;
using System.Threading.Tasks;
using AutoMapper;
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
            PersonViewModel personVm = new PersonViewModel();

            try
            {
                TaskResult<User> result = await this.personService.GetUser(id);
                personVm = null;
                if (result.Succeeded)
                {
                    personVm = new PersonViewModel
                    {
                        Id = new Guid(result.Data.Id),
                        FirstName = result.Data.GivenName,
                        LastName = result.Data.Surname,
                        Email = result.Data.Mail,
                        StreetAddress = result.Data.StreetAddress,
                        PostalCode = result.Data.PostalCode,
                        City = result.Data.City,
                        Country = result.Data.Country,
                    };
                    //todo deze hardcoded referenties laten verwijzen naar 
                    if (result.Data.AdditionalData.ContainsKey($"extension_4e6dae7dd1c74eac85fefc6da42e7b61_UserRole")){
                        UserRole role;
                        Enum.TryParse(
                            result.Data.AdditionalData[$"extension_4e6dae7dd1c74eac85fefc6da42e7b61_UserRole"]
                                .ToString(), out role);
                        personVm.UserRole = role.ToString();
                    }
                    
                    if (result.Data.AdditionalData.ContainsKey("extension_4e6dae7dd1c74eac85fefc6da42e7b61_DateOfBirth")){
                        personVm.DateOfBirth = result.Data.AdditionalData["extension_4e6dae7dd1c74eac85fefc6da42e7b61_DateOfBirth"].ToString();
                    } 
                    if (result.Data.AdditionalData.ContainsKey("extension_4e6dae7dd1c74eac85fefc6da42e7b61_PhoneNumber")){
                        personVm.PhoneNumber = result.Data.AdditionalData["extension_4e6dae7dd1c74eac85fefc6da42e7b61_PhoneNumber"].ToString();
                    }

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