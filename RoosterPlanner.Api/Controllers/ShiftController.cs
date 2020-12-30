using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Api.Models.Constants;
using RoosterPlanner.Common.Config;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Types;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using Shift = RoosterPlanner.Models.Shift;
using Task = RoosterPlanner.Models.Task;
using Type = RoosterPlanner.Api.Models.Type;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly AzureAuthenticationConfig azureB2CConfig;
        private readonly ILogger<ShiftController> logger;
        private readonly IShiftService shiftService;
        private readonly IProjectService projectService;
        private readonly IPersonService personService;
        private readonly ITaskService taskService;
        private readonly IAvailabilityService availabilityService;

        public ShiftController(
            ILogger<ShiftController> logger,
            IShiftService shiftService,
            IProjectService projectService,
            ITaskService taskService,
            IPersonService personService,
            IOptions<AzureAuthenticationConfig> azureB2CConfig,
            IAvailabilityService availabilityService)

        {
            this.logger = logger;
            this.shiftService = shiftService;
            this.projectService = projectService;
            this.personService = personService;
            this.taskService = taskService;
            this.azureB2CConfig = azureB2CConfig.Value;
            this.availabilityService = availabilityService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<List<ShiftViewModel>>> GetShiftsAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid id");
            try
            {
                TaskListResult<Shift> result = await shiftService.GetShiftsAsync(projectId);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null || result.Data.Count == 0)
                    return Ok(new List<ShiftViewModel>());
                List<ShiftViewModel> shiftVmList = result.Data.Select(ShiftViewModel.CreateVm)
                    .ToList();
                return Ok(shiftVmList);
            }
            catch (Exception ex)
            {
                string message = GetType()
                    .Name + "Error in " + nameof(GetShiftAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpGet("{projectId}/{userId}/{date}")]
        public async Task<ActionResult<List<ShiftViewModel>>> GetShiftsAsync(Guid projectId, Guid userId, DateTime date)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid projectId");
            if (userId == Guid.Empty)
                return BadRequest("No valid userI");
            try
            {
                TaskListResult<Shift> result = await shiftService.GetShiftsAsync(projectId, userId, date);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null || result.Data.Count == 0)
                    return Ok(new List<ShiftViewModel>());
                List<ShiftViewModel> shiftVmList = result.Data.Select(ShiftViewModel.CreateVm)
                    .ToList();
                return Ok(shiftVmList);
            }
            catch (Exception ex)
            {
                string message = GetType()
                    .Name + "Error in " + nameof(GetShiftAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpGet("{projectId}/{date}")]
        public async Task<ActionResult<List<ShiftViewModel>>> GetShiftsAsync(Guid projectId, DateTime date)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid projectId");
            try
            {
                TaskListResult<Shift> result = await shiftService.GetShiftsAsync(projectId, date);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null || result.Data.Count == 0)
                    return Ok(new List<ShiftViewModel>());
                List<ShiftViewModel> shiftVmList = result.Data.Select(ShiftViewModel.CreateVm)
                    .ToList();
                return Ok(shiftVmList);
            }
            catch (Exception ex)
            {
                string message = GetType()
                    .Name + "Error in " + nameof(GetShiftAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShiftViewModel>> GetShiftAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();
            try
            {
                TaskResult<Shift> result = await shiftService.GetShiftAsync(id);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null)
                    return NotFound();
                return Ok(ShiftViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType()
                    .Name + "Error in " + nameof(GetShiftAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpGet("schedule/{id}")]
        public async Task<ActionResult<ScheduleDataViewModel>> GetScheduleAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();
            try
            {
                TaskResult<Shift> shiftResult = await shiftService.GetShiftWithAvailabilitiesAsync(id);
                if (!shiftResult.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = shiftResult.Message});
                if (shiftResult.Data == null)
                    return NotFound();

                List<ScheduleViewModel> schedules = new List<ScheduleViewModel>();

                //get a list with all days this week
                var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                int numberOfDaysBetweenNowAndStart =
                    shiftResult.Data.Date.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
                DateTime firstDateThisWeek =
                    shiftResult.Data.Date.Subtract(TimeSpan.FromDays(numberOfDaysBetweenNowAndStart));
                List<DateTime> allDaysThisWeek = new List<DateTime>();
                for (int i = 0; i < 7; i++)
                    allDaysThisWeek.Add(firstDateThisWeek.AddDays(i));

                foreach (Availability availability in shiftResult.Data.Availabilities
                    .Where(a => a.Type == AvailibilityType.Ok || a.Type == AvailibilityType.Scheduled)
                ) //filter for people that are registerd to be able to work
                {
                    //list all availabilities in this project of this person
                    Task<TaskListResult<Availability>> availabilities =
                        this.availabilityService.FindAvailabilitiesAsync(
                            availability.Participation.ProjectId,
                            availability.Participation.PersonId);

                    //lookup person information in B2C
                    Task<TaskResult<User>> person = personService.GetUserAsync(availability.Participation.PersonId);
                    await System.Threading.Tasks.Task.WhenAll(availabilities, person);

                    //see if person is scheduled this day and this shift
                    if (availabilities.Result?.Data == null || availabilities.Result.Data.Count <= 0) continue;
                    int numberOfTimeScheduledThisDay = availabilities.Result.Data
                        .Where(a => a.Shift.Date == shiftResult.Data.Date)
                        .Count(a => a.Type == AvailibilityType.Scheduled);

                    bool scheduledThisShift = availabilities.Result.Data.FirstOrDefault(a =>
                        a.ShiftId == id && a.Type == AvailibilityType.Scheduled) != null;

                    //calculate the number of hours person is scheduled this week
                    double numberOfHoursScheduleThisWeek = (availabilities.Result.Data)
                        .Where(a => a.Type == AvailibilityType.Scheduled && allDaysThisWeek.Contains(a.Shift.Date))
                        .Sum(availability1 =>
                            availability1.Shift.EndTime.Subtract(availability1.Shift.StartTime).TotalHours);

                    //add scheduleViewmodel to list
                    schedules.Add(new ScheduleViewModel
                    {
                        Person = PersonViewModel.CreateVmFromUser(person.Result.Data,
                            Extensions.GetInstance(azureB2CConfig)),
                        NumberOfTimesScheduledThisProject = numberOfTimeScheduledThisDay,
                        ScheduledThisDay = numberOfTimeScheduledThisDay > 0,
                        ScheduledThisShift = scheduledThisShift,
                        AvailabilityId = availability.Id,
                        Preference = availability.Preference,
                        Availabilities = (await availabilities).Data
                            .Where(a => a.Shift.Date == shiftResult.Data.Date)
                            .Select(availability1 => AvailabilityViewModel.CreateVm(availability1))
                            .ToList(),
                        HoursScheduledThisWeek = numberOfHoursScheduleThisWeek,
                        Employability = availability.Participation.MaxWorkingHoursPerWeek
                    });
                }

                ScheduleDataViewModel vm = new ScheduleDataViewModel()
                {
                    Schedules = schedules,
                    Shift = ShiftViewModel.CreateVm(shiftResult.Data)
                };

                return Ok(vm);
            }
            catch (Exception ex)
            {
                string message = GetType()
                    .Name + "Error in " + nameof(GetScheduleAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpPost]
        public async Task<ActionResult<List<ShiftViewModel>>> SaveShiftsAsync(List<ShiftViewModel> shiftViewModels)
        {
            if (shiftViewModels == null || shiftViewModels.Count == 0)
                return BadRequest("No valid shifts received");

            try
            {
                List<Shift> shifts = shiftViewModels.Select(ShiftViewModel.CreateShift)
                    .ToList();
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);

                if (shifts[0] != null)
                {
                    //get project and get task from db
                    Project project = (await projectService.GetProjectDetailsAsync(shifts[0]
                        .ProjectId)).Data;
                    Task task = null;
                    if (shifts[0]
                        .TaskId != null)
                        task = (await taskService.GetTaskAsync((Guid) shifts[0]
                            .TaskId)).Data;

                    foreach (Shift shift in shifts)
                    {
                        if (shift.Id != Guid.Empty || shift.TaskId == null || shift.ProjectId == Guid.Empty)
                            shifts.Remove(shift);

                        // check if projectId and taskId differs from above? getproject/task => add project and task to shift
                        if (project.Id != shift.ProjectId)
                            project = (await projectService.GetProjectDetailsAsync(shift.ProjectId)).Data;

                        if (task != null && shift.TaskId != null && task.Id != shift.TaskId)
                            task = (await taskService.GetTaskAsync((Guid) shift.TaskId)).Data;

                        if (project == null || task == null)
                            shifts.Remove(shift);
                        shift.Project = project;
                        shift.Task = task;
                        shift.LastEditBy = oid;
                    }
                }

                if (shifts.Count != shiftViewModels.Count)
                    return UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = "Could not covert al viewmodels to shifts"});

                TaskListResult<Shift> result = await shiftService.CreateShiftsAsync(shifts);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});

                List<ShiftViewModel> createdVm = result.Data.Select(ShiftViewModel.CreateVm)
                    .ToList();
                return Ok(createdVm);
            }
            catch (Exception ex)
            {
                string message = GetType()
                    .Name + "Error in " + nameof(SaveShiftsAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpPut]
        public async Task<ActionResult<ShiftViewModel>> UpdateShiftAsync(ShiftViewModel shiftViewModel)
        {
            if (shiftViewModel == null || shiftViewModel.Id == Guid.Empty || shiftViewModel.Project == null ||
                shiftViewModel.Task == null)
                return BadRequest("No valid Shift received");
            if (shiftViewModel.Task.Id == Guid.Empty || shiftViewModel.Project.Id == Guid.Empty)
                return BadRequest("ProjectId and or TaskId cannot be empty");
            try
            {
                Shift oldShift = (await shiftService.GetShiftAsync(shiftViewModel.Id)).Data;
                if (oldShift == null)
                    return NotFound("Shift not found");

                if (oldShift.ProjectId != shiftViewModel.Project.Id)
                    return BadRequest("Cannot change the project of a shift");

                Shift updatedShift = ShiftViewModel.CreateShift(shiftViewModel);
                if (updatedShift == null)
                    return BadRequest("Unable to convert ShiftViewModel to Shift");

                Project project = (await projectService.GetProjectDetailsAsync(updatedShift.ProjectId)).Data;
                Task task = null;
                if (updatedShift.TaskId != null)
                    task = (await taskService.GetTaskAsync((Guid) updatedShift.TaskId)).Data;

                if (project == null || task == null)
                    return NotFound("Project and or Task Not found");

                oldShift.StartTime = updatedShift.StartTime;
                oldShift.EndTime = updatedShift.EndTime;
                oldShift.ParticipantsRequired = updatedShift.ParticipantsRequired;
                oldShift.Task = task;
                oldShift.Project = project;
                //cannot update Project, Id or Date by design
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldShift.LastEditBy = oid;

                TaskResult<Shift> result = await shiftService.UpdateShiftAsync(oldShift);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(ShiftViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType()
                    .Name + "Error in " + nameof(UpdateShiftAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpDelete]
        public async Task<ActionResult<ShiftViewModel>> RemoveShiftAsync(Guid shiftId)
        {
            if (shiftId == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskResult<Shift> shift = await shiftService.GetShiftAsync(shiftId);
                if (shift == null)
                    return NotFound("Shift not found");

                TaskResult<Shift> result = await shiftService.RemoveShiftAsync(shift.Data);

                return !result.Succeeded
                    ? UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = result.Message})
                    : Ok(result);
            }
            catch (Exception ex)
            {
                string message = GetType()
                    .Name + "Error in " + nameof(RemoveShiftAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
    }
}