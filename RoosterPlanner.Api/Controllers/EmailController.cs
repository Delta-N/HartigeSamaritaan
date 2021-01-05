using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Email;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Types;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using Person = RoosterPlanner.Models.Person;
using Type = RoosterPlanner.Api.Models.Type;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> logger;
        private readonly IEmailService emailService;
        private readonly IPersonService personService;
        private readonly IParticipationService participationService;

        public EmailController(ILogger<EmailController> logger, IEmailService emailService, IPersonService
            personService, IParticipationService participationService)
        {
            this.logger = logger;
            this.emailService = emailService;
            this.personService = personService;
            this.participationService = participationService;
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

                foreach (Participation participation in participations.Data.Where(participation =>
                    participation.Availabilities.Count > 0))
                {
                    TaskResult<User> user = await personService.GetUserAsync(participation.PersonId);
                    TaskResult<Person> person = await personService.GetPersonAsync(participation.PersonId);

                    if (!user.Succeeded || !person.Succeeded || person.Data.PushDisabled) continue;

                    string email = user.Data.Identities.FirstOrDefault()?.IssuerAssignedId;
                    if (email == null) continue;

                    string body = null;
                    foreach (Availability availability in participation.Availabilities
                        .Where(a => a.Type == AvailibilityType.Scheduled && !a.PushEmailSend)
                        .OrderBy(a => a.Shift.Date))
                    {
                        if (body == null)
                        {
                            body += "Beste " + user.Data.DisplayName + ",<br><br>";
                            body += "Je bent ingeroosterd voor de volgende diensten:<br><br>";
                        }
                        
                        body += "<b>" +availability.Shift.Date.ToString("dddd, dd MMMM yyyy")+" - "+ availability.Shift.Task.Name + "</b><br>" +
                                "Van: " + availability.Shift.StartTime.ToString("hh\\:mm") + "uur<br>" +
                                "Tot: " + availability.Shift.EndTime.ToString("hh\\:mm") + "uur<br><br>";

                        //change attribute in db
                        availability.PushEmailSend = true;
                    }

                    await participationService.UpdateParticipationAsync(participation);
                    
                    if (body == null) continue;
                    body += "Lees vooraf a.u.b. de instructies voor deze taken goed door.<br><br>";
                    body += "Groeten, <br><br> Het Hartige Samaritaan Team";
                    emailService.SendEmail(email,
                        "Je bent ingeroosterd",
                        body,
                        true, null);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(RequestAvailability);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpPost("availability/{projectId}")]
        public async Task<ActionResult<bool>> RequestAvailability(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid id.");

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

                    string body = "Beste " + user.Data.DisplayName + ",<br><br>";
                    body += "Je kunt jezelf opgeven voor nieuwe diensten voor het project: <b>" +
                            participation.Project?.Name + "</b><br><br>";
                    body += "Groeten, <br><br> Het Hartige Samaritaan Team";

                    emailService.SendEmail(email,
                        "Je kunt je opgeven voor diensten",
                        body,
                        true, null);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(RequestAvailability);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
    }
}