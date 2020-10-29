using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipationController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IParticipationService participationService;
        private readonly IProjectService projectService;

        public ParticipationController(ILogger logger, IParticipationService participationService,
            IProjectService projectService)
        {
            this.logger = logger;
            this.participationService = participationService;
            this.projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserParticipation(Guid personGuid)
        {
            try
            {
                if (personGuid == Guid.Empty) return BadRequest("No valid id.");
                TaskListResult<Participation> result = await participationService.GetParticipations(personGuid);

                if (!result.Succeeded) return UnprocessableEntity();

                List<ParticipationViewModel> participationViewModels = result.Data
                    .Select(ParticipationViewModel.CreateVm)
                    .ToList();

                return Ok(participationViewModels);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ParticipationController: Error occured.");
                Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> Save(ParticipationViewModel participationViewModel)
        {
            if (participationViewModel == null || participationViewModel.Person==null|| participationViewModel.Project==null)
            {
                return BadRequest("Er is geen geldige participation ontvangen");
            }

            Task<TaskResult<Participation>> result = null;
            try
            {
                Participation participation = ParticipationViewModel.CreateParticipation(participationViewModel);
                if (participation.Id==Guid.Empty)
                {
                    //create participation
                    result = participationService.AddParticipationAsync(participation.PersonId,participation.ProjectId, participation.MaxWorkingHoursPerWeek);
                }
                else
                {
                    //update participation todo 
                }

                if (result.Result.Succeeded)
                    return Ok(ParticipationViewModel.CreateVm(result.Result.Data));
                return UnprocessableEntity(participationViewModel);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ParticipationController: Error occured.");
                Response.Headers.Add("message", ex.Message);
            }
            return NoContent();
        }
    }
}