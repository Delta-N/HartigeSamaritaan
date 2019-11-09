using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Service;

namespace RoosterPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersController : ControllerBase
    {
        protected IMapper Mapper { get; set; }
        public IProjectService ProjectService { get; set; }

        public IParticipationService ParticipationService { get; set; }
        public IAzureB2CService AzureB2CService { get; set; }
        public IPersonService PersonService { get; set; }

        public VolunteersController(IMapper mapper, IProjectService projectService, IParticipationService participationService, IAzureB2CService azureB2CService, IPersonService personService)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            ProjectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            ParticipationService = participationService ?? throw new ArgumentNullException(nameof(participationService));
            AzureB2CService = azureB2CService ?? throw new ArgumentNullException(nameof(azureB2CService));
            PersonService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        [HttpGet("getprojects")]
        public async Task<ActionResult<IEnumerable<Projects>>> GetProjects()
        {
            var projects = await ProjectService.GetActiveProjectsAsync();
            return  projects.Data.Select(i => Mapper.Map<Projects>(i)).ToList();
        }

        [HttpPost("setparticipation/{oid}/{projectId}")]
        public async Task<ActionResult> SetParticipation(Guid oid, Guid projectId)
        {
            await ParticipationService.AddParticipationAsync(oid, projectId);
            return Ok();
        }

        [HttpGet("triggerupdate/{oid}")]
        public async Task<ActionResult> TriggerUpdate(Guid oid)
        {
            var user = await AzureB2CService.GetUserAsync(oid);

            await PersonService.UpdatePersonName(oid, user.Data.DisplayName);

            return Ok();
        }
    }
}