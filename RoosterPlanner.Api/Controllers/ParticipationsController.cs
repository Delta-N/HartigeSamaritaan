using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Models;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using Type = RoosterPlanner.Api.Models.Type;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipationsController : ControllerBase
    {
        private readonly ILogger<ParticipationsController> logger;
        private readonly IParticipationService participationService;

        public ParticipationsController(ILogger<ParticipationsController> logger,
            IParticipationService participationService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.participationService =
                participationService ?? throw new ArgumentNullException(nameof(participationService));
        }

        [HttpGet("{personId}")]
        public async Task<ActionResult<List<ParticipationViewModel>>> GetUserParticipationAsync(Guid personId)
        {
            if (personId == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskListResult<Participation> result = await participationService.GetUserParticipationsAsync(personId);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data.Count == 0)
                    return Ok(new List<ParticipationViewModel>());

                List<ParticipationViewModel> participationViewModels = result.Data
                    .Select(ParticipationViewModel.CreateVm)
                    .ToList();

                return Ok(participationViewModels);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetParticipationAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpGet("GetParticipation/{personid}/{projectid}")]
        public async Task<ActionResult<ParticipationViewModel>> GetParticipationAsync(Guid personId, Guid projectId)
        {
            if (personId == Guid.Empty || projectId == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<Participation>
                    result = await participationService.GetParticipationAsync(personId, projectId);

                if (!result.Succeeded)
                    if (result.Data == null)
                        return NoContent();
                ParticipationViewModel participationViewModel = ParticipationViewModel.CreateVm(result.Data);

                return Ok(participationViewModel);

            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetParticipationAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpPost]
        public async Task<ActionResult<ParticipationViewModel>> SaveParticipationAsync(
            [FromBody] ParticipationViewModel participationViewModel)
        {
            if (participationViewModel?.Person == null || participationViewModel.Project == null)
                return BadRequest("No valid participation received");

            TaskResult<Participation> result = null;
            try
            {
                Participation participation = ParticipationViewModel.CreateParticipation(participationViewModel);
                participation.LastEditBy = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);

                if (participation.Id == Guid.Empty)
                    //create participation
                    result = await participationService.AddParticipationAsync(participation);

                if (result == null || !result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result?.Message});
                return Ok(ParticipationViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SaveParticipationAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpPut]
        public async Task<ActionResult<ParticipationViewModel>> UpdateParticipationAsync(
            [FromBody] ParticipationViewModel participationViewModel)
        {
            if (participationViewModel?.Person == null ||
                participationViewModel.Person.Id == Guid.Empty ||
                participationViewModel.Project == null ||
                participationViewModel.Project.Id == Guid.Empty)
                return BadRequest("No valid participation received");

            TaskResult<Participation> result = new TaskResult<Participation>();
            try
            {
                Participation oldParticipation =
                    participationService
                        .GetParticipationAsync(participationViewModel.Person.Id, participationViewModel.Project.Id)
                        .Result
                        .Data;
                Participation updatedParticipation = ParticipationViewModel.CreateParticipation(participationViewModel);

                if (oldParticipation.ProjectId != updatedParticipation.ProjectId)
                {
                    result.Message = "A participation cannot change project";
                    return BadRequest(result);
                }

                if (oldParticipation.PersonId != updatedParticipation.PersonId)
                {
                    result.Message = "A participation cannot change user";
                    return BadRequest(result);
                }

                oldParticipation.Availabilities = updatedParticipation.Availabilities;
                oldParticipation.IsWantedBy = updatedParticipation.IsWantedBy;
                oldParticipation.WantsToWorkWith = updatedParticipation.WantsToWorkWith;
                oldParticipation.MaxWorkingHoursPerWeek = updatedParticipation.MaxWorkingHoursPerWeek;

                oldParticipation.Person = null;
                oldParticipation.Project = null;
                oldParticipation.LastEditBy = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);

                result = await participationService.UpdateParticipationAsync(oldParticipation);

                if (result == null || !result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result?.Message});

                result.Data.Person = updatedParticipation.Person;
                result.Data.Project = updatedParticipation.Project;
                return Ok(ParticipationViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(UpdateParticipationAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ParticipationViewModel>> RemoveParticipationAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<Participation> participation = await participationService.GetParticipationAsync(id);
                if (!participation.Succeeded) BadRequest("Invalid participation");

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                if (oid == null)
                    return BadRequest("Invalid User");

                if (oid != participation.Data.PersonId.ToString())
                    return Unauthorized();

                //gebruiker mag participation verwijderen
                TaskResult<Participation> result =
                    await participationService.RemoveParticipationAsync(participation.Data);

                return !result.Succeeded ? UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message}) : Ok(result);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(RemoveParticipationAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
    }
}