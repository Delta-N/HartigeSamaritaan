using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Types;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using Person = RoosterPlanner.Models.Person;
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
        private readonly IAvailabilityService availabilityService;
        private readonly IPersonService personService;

        public ParticipationsController(
            ILogger<ParticipationsController> logger,
            IParticipationService participationService,
            IAvailabilityService availabilityService,
            IPersonService personService)
        {
            this.logger = logger;
            this.participationService = participationService;
            this.availabilityService = availabilityService;
            this.personService = personService;
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

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<List<ParticipationViewModel>>> GetParticipationsAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskListResult<Participation> result = await participationService.GetParticipationsAsync(projectId);
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
                string message = GetType().Name + "Error in " + nameof(GetParticipationsAsync);
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
                Participation participation = (await participationService.GetParticipationAsync(
                    participationViewModel.Person.Id,
                    participationViewModel.Project.Id)).Data;
                if (participation != null)
                {
                    participation.Active = true;
                    participation.LastEditBy = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                    result = await participationService.UpdateParticipationAsync(participation);
                }

                else
                {
                    participation = ParticipationViewModel.CreateParticipation(participationViewModel);
                    participation.LastEditBy = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                    if (participation.Id == Guid.Empty)
                        //create participation
                        result = await participationService.AddParticipationAsync(participation);
                }

                if (result == null || !result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel
                    {
                        Type = Type.Error, Message = result?.Message
                    });
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
                Participation oldParticipation = (await participationService
                        .GetParticipationAsync(participationViewModel.Person.Id, participationViewModel.Project.Id)
                    ).Data;
                if (!oldParticipation.RowVersion.SequenceEqual(participationViewModel.RowVersion))
                    return BadRequest("Outdated entity received");

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
                oldParticipation.MaxWorkingHoursPerWeek = updatedParticipation.MaxWorkingHoursPerWeek;
                oldParticipation.Active = updatedParticipation.Active;
                oldParticipation.Remark = updatedParticipation.Remark;
                oldParticipation.Person = null;
                oldParticipation.Project = null;
                oldParticipation.LastEditBy = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);

                result = await participationService.UpdateParticipationAsync(oldParticipation);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});

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
                if (!participation.Succeeded)
                    BadRequest("Invalid participation");

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                if (oid == null)
                    return BadRequest("Invalid User");

                if (oid != participation.Data.PersonId.ToString())
                    return Unauthorized();

                //controleer of gebruiker niet ingeroosterd staat
                TaskListResult<Availability>
                    availabilities = await availabilityService.GetActiveAvailabilities(id);
                if (availabilities.Data.Count > 0)
                    return BadRequest("Je kan je niet uitschrijven voor dit project. Je bent nog ingepland.");

                //gebruiker mag participation verwijderen
                participation.Data.Active = false;
                TaskResult<Participation> result =
                    await participationService.UpdateParticipationAsync(participation.Data);

                return !result.Succeeded
                    ? UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message})
                    : Ok(result);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(RemoveParticipationAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpPost("Schedule/{projectId}")]
        public async Task<ActionResult<bool>> SendSchedule(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                
                TaskListResult<Participation> participations =
                    await participationService.GetParticipationsWithAvailabilitiesAsync(projectId);
                if (!participations.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = participations.Message});
                CultureInfo culture = new CultureInfo("en-US");
                
                foreach (Participation participation in participations.Data.Where(participation =>
                    participation.Availabilities.Count > 0))
                {
                    TaskResult<User> user = await personService.GetUserAsync(participation.PersonId);
                    TaskResult<Person> person = await personService.GetPersonAsync(participation.PersonId);

                    if (!user.Succeeded || !person.Succeeded || person.Data.PushDisabled) continue;

                    string email = user.Data.Identities.FirstOrDefault()?.IssuerAssignedId;
                    if (email == null) continue;

                    StringBuilder sb = new StringBuilder();
                    string DateFormat = "yyyyMMddTHHmmssZ";
                    string now = DateTime.Now.ToUniversalTime().ToString(DateFormat);
                    sb.AppendLine("BEGIN:VCALENDAR");
                    sb.AppendLine("PRODID:-//Compnay Inc//Product Application//EN");
                    sb.AppendLine("VERSION:2.0");
                    sb.AppendLine("METHOD:PUBLISH");
                    

                    string body = null;
                    foreach (Availability availability in participation.Availabilities
                        .Where(a => a.Type == AvailibilityType.Scheduled && !a.PushEmailSend)
                        .OrderBy(a => a.Shift.Date))
                    {

                        DateTime dtStart = availability.Shift.Date + availability.Shift.StartTime;
                        DateTime dtEnd = availability.Shift.Date + availability.Shift.EndTime;
                        sb.AppendLine("BEGIN:VEVENT");
                        sb.AppendLine("DTSTART:" + dtStart.ToUniversalTime().ToString(DateFormat));
                        sb.AppendLine("DTEND:" + dtEnd.ToUniversalTime().ToString(DateFormat));
                        sb.AppendLine("DTSTAMP:" + now);
                        sb.AppendLine("UID:" + Guid.NewGuid());
                        sb.AppendLine("CREATED:" + now);
                        sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + availability.Shift.Task.Description);
                        //sb.AppendLine("DESCRIPTION:" + res.Details);
                        sb.AppendLine("LAST-MODIFIED:" + now);
                        sb.AppendLine("LOCATION:" + participation.Project.Address+" "+participation.Project.City);
                        sb.AppendLine("SEQUENCE:0");
                        sb.AppendLine("STATUS:CONFIRMED");
                        sb.AppendLine("SUMMARY:" + availability.Shift.Task.Name);
                        sb.AppendLine("TRANSP:OPAQUE");
                        sb.AppendLine("END:VEVENT");
                        
                        if (body == null)
                        {
                            body += "Beste " + user.Data.DisplayName + ",<br><br>";
                            body += "Je bent zojuist ingeroosterd voor de volgende diensten:<br><br>";
                        }

                        body += "<b>" + availability.Shift.Date.ToString("dddd, dd MMMM yyyy",culture) + " - " +
                                availability.Shift.Task.Name + "</b><br>" +
                                "Van: " + availability.Shift.StartTime.ToString("hh\\:mm") + "uur<br>" +
                                "Tot: " + availability.Shift.EndTime.ToString("hh\\:mm") + "uur<br><br>";
                        
                        //change attribute in db
                        availability.PushEmailSend = true;
                    }

                    sb.AppendLine("END:VCALENDAR");
                    var calendarBytes = Encoding.UTF8.GetBytes(sb.ToString());
                    MemoryStream ms = new MemoryStream(calendarBytes);
                    System.Net.Mail.Attachment attachment =
                        new System.Net.Mail.Attachment(ms, "event.ics", "text/calendar");

                    await participationService.UpdateParticipationAsync(participation);

                    if (body == null) continue;
                    body += "Bekijk via <u><a href=\"http://localhost:4200/schedule/"+participation.Id+"\">deze link</a></u> alle shifts waarvoor je staat ingeroosterd.<br>"; //todo deze link aanpassen zodra dns definitief is
                    body += "Via deze pagina kun je ook de beschrijving en instructies voor je shift zien.<br><br>";
                    body += "Hartige groetjes en tot ziens!";

                    participationService.SendEmail(email,
                        "Je bent ingeroosterd",
                        body,
                        true, null,attachment);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SendEmailAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpPost("availability/{projectId}")]
        public async Task<ActionResult<bool>> SendEmailAsync(Guid projectId, MessageViewModel emailMessage)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid id.");
            if (emailMessage?.Body == null || emailMessage.Subject == null)
                return BadRequest("No valid message.");

            try
            {
                TaskListResult<Participation> participations =
                    await participationService.GetParticipationsAsync(projectId);
                if (!participations.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = participations.Message});

                foreach (Participation participation in participations.Data)
                {
                    TaskResult<User> user = await personService.GetUserAsync(participation.PersonId);
                    TaskResult<Person> person = await personService.GetPersonAsync(participation.PersonId);

                    if (!user.Succeeded || !person.Succeeded || person.Data.PushDisabled) continue;

                    string email = user.Data.Identities.FirstOrDefault()?.IssuerAssignedId;
                    if (email == null) continue;
                    emailMessage.Body = emailMessage.Body.Replace("\n", "<br>");

                    participationService.SendEmail(email,
                        emailMessage.Subject,
                        emailMessage.Body,
                        true, null,null);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SendEmailAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
    }
}