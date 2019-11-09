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

        public IShiftService ShiftService { get; set; }

        public VolunteersController(IMapper mapper, 
                                    IProjectService projectService, 
                                    IParticipationService participationService, 
                                    IAzureB2CService azureB2CService, 
                                    IPersonService personService,
                                    IShiftService shiftService)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            ProjectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            ParticipationService = participationService ?? throw new ArgumentNullException(nameof(participationService));
            AzureB2CService = azureB2CService ?? throw new ArgumentNullException(nameof(azureB2CService));
            PersonService = personService ?? throw new ArgumentNullException(nameof(personService));
            ShiftService = shiftService ?? throw new ArgumentNullException(nameof(shiftService));
        }

        [HttpGet("getprojects")]
        public async Task<ActionResult<IEnumerable<ProjectViewModel>>> Get()
        {
            var projects = await ProjectService.GetActiveProjectsAsync();
            return projects.Data.Select(i => Mapper.Map<ProjectViewModel>(i)).ToList();
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

        [HttpGet("getshifts/{projectId}")]
        public async Task<ActionResult<List<ShiftViewModel>>> GetShifts(Guid projectId)
        {
            var shifts = await ShiftService.GetActiveShiftsForProjectAsync(projectId);
            return shifts.Data.Select(i => Mapper.Map<ShiftViewModel>(i)).ToList();
        }
    }
}