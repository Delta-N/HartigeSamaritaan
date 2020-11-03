using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public ParticipationController(ILogger logger, IParticipationService participationService)
        {
            this.logger = logger;
            this.participationService = participationService;
        }

        [HttpGet]
        public async Task<ActionResult> GetUserParticipation(Guid personGuid)
        {
            try
            {
                if (personGuid == Guid.Empty) return BadRequest("No valid id.");
                TaskListResult<Participation> result = await participationService.GetUserParticipations(personGuid);

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

        [HttpGet]
        [Route("GetParticipation/{personGuid}/{projectGuid}")]
        public async Task<ActionResult> GetParticipation(Guid personGuid, Guid projectGuid)
        {
            try
            {
                if (personGuid == Guid.Empty || projectGuid == Guid.Empty) return BadRequest("No valid id.");
                TaskResult<Participation> result = await participationService.GetParticipation(personGuid, projectGuid);

                if (!result.Succeeded) return UnprocessableEntity();

                ParticipationViewModel participationViewModel = ParticipationViewModel.CreateVm(result.Data);


                return Ok(participationViewModel);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ParticipationController: Error occured.");
                Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        [HttpPost]
        public ActionResult Save([FromBody] ParticipationViewModel participationViewModel)
        {
            if (participationViewModel == null || participationViewModel.Person == null ||
                participationViewModel.Project == null)
            {
                return BadRequest("Er is geen geldige participation ontvangen");
            }

            Task<TaskResult<Participation>> result = null;
            try
            {
                Participation participation = ParticipationViewModel.CreateParticipation(participationViewModel);
                participation.LastEditBy = GetOid(HttpContext.User.Identity as ClaimsIdentity);
                participation.LastEditDate = DateTime.UtcNow;

                if (participation.Id == Guid.Empty)
                {
                    //create participation
                    result = participationService.AddParticipationAsync(participation);
                }

                if (result != null && result.Result.Succeeded)
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

        [HttpPatch]
        public ActionResult Update([FromBody] ParticipationViewModel participationViewModel)
        {
            if (participationViewModel == null || participationViewModel.Person == null ||
                participationViewModel.Project == null)
            {
                return BadRequest("Er is geen geldige participation ontvangen");
            }

            Task<TaskResult<Participation>> result = null;
            try
            {
                Participation oldParticipation =
                    participationService
                        .GetParticipation(participationViewModel.Person.Id, participationViewModel.Project.Id).Result
                        .Data;
                Participation updatedParticipation = ParticipationViewModel.CreateParticipation(participationViewModel);

                if (oldParticipation.ProjectId != updatedParticipation.ProjectId)
                {
                    result.Result.Succeeded = false;
                    result.Result.Message = "A participation cannot change project";
                    return BadRequest(result);
                }

                if (oldParticipation.PersonId != updatedParticipation.PersonId)
                {
                    result.Result.Succeeded = false;
                    result.Result.Message = "A participation cannot change user";
                    return BadRequest(result);
                }

                oldParticipation.Availabilities = updatedParticipation.Availabilities;
                oldParticipation.IsWantedBy = updatedParticipation.IsWantedBy;
                oldParticipation.WantsToWorkWith = updatedParticipation.WantsToWorkWith;
                oldParticipation.MaxWorkingHoursPerWeek = updatedParticipation.MaxWorkingHoursPerWeek;

                oldParticipation.Person = null;
                oldParticipation.Project = null;
                oldParticipation.LastEditBy = GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldParticipation.LastEditDate = DateTime.UtcNow;

                result = participationService.UpdateParticipation(oldParticipation);

                if (result != null && result.Result.Succeeded)
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

        [HttpDelete]
        public async Task<IActionResult> RemoveParticipation(Guid id)
        {
            try
            {
                TaskResult<Participation> participation = participationService.GetParticipation(id);
                if (!participation.Succeeded)
                {
                    BadRequest("Invalid participation");
                }

                var oid = GetOid(HttpContext.User.Identity as ClaimsIdentity);
                if (oid == null) return BadRequest("Invalid User");

                if (oid == participation.Data.PersonId.ToString())
                {
                    //gebruiker mag participation verwijderen
                    var result = await participationService.RemoveParticipation(participation.Data);
                    if (result.Succeeded)
                        return Ok(result);
                    return Problem();
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ParticipationController: Error occured.");
                Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        private static string GetOid(ClaimsIdentity claimsIdentity)
        {
            var identity = claimsIdentity;
            string oid = null;
            if (identity != null)
                oid = identity.Claims.FirstOrDefault(c =>
                        c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
                    ?.Value;
            return oid;
        }
    }
}