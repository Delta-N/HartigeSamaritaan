﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using RoosterPlanner.Api.Models.EntityViewModels;
using RoosterPlanner.Api.Models.HelperViewModels;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Models.Models.Types;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using RoosterPlanner.Service.Services;
using Shift = RoosterPlanner.Models.Models.Shift;
using Task = RoosterPlanner.Models.Models.Task;
using Type = RoosterPlanner.Api.Models.HelperViewModels.Type;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController(
    ILogger<ShiftController> logger,
    IShiftService shiftService,
    IProjectService projectService,
    ITaskService taskService,
    IPersonService personService,
    IAvailabilityService availabilityService,
    string b2CExtentionApplicationId)
    : ControllerBase {
        /// <summary>
        /// Makes a request towards the services layer to get distinct data from a project.
        /// Only Boardmembers and Committemember can request distinct data.
        /// Examples of distinct data are: Tasks, dates, startimes, endtimes and participantsrequired.
        /// Used in the shiftfilter.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpGet("unique/{projectId}")]
        public async Task<ActionResult<ShiftData>> GetUniqueDataAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid id");
            try
            {
                TaskResult<ShiftData> result = await shiftService.GetUniqueDataAsync(projectId);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                string message = GetType()
                    .Name + "Error in " + nameof(GetUniqueDataAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Search for shifts based on a filter.
        /// Only Boardmembers and Committeemembers can search for shifts.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpPost("search")]
        public async Task<ActionResult<SearchResultViewModel<ShiftViewModel>>> GetShiftsAsync(ShiftFilter filter)
        {
            if (filter == null)
                return BadRequest("No filter received");
            try
            {
                TaskListResult<Shift> result = await shiftService.GetShiftsAsync(filter);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null || result.Data.Count == 0)
                    return Ok(new SearchResultViewModel<ShiftViewModel>(0, new List<ShiftViewModel>()));

                List<ShiftViewModel> shiftVmList = result.Data.Select(ShiftViewModel.CreateVm)
                    .ToList();
                return Ok(new SearchResultViewModel<ShiftViewModel>(filter.TotalItemCount, shiftVmList));
            }
            catch (Exception ex)
            {
                string message = GetType()
                    .Name + "Error in " + nameof(GetShiftAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer for all shifts based on a projectId, userId and a date.
        /// Used on the 'beschikbaarheid opgeven' page.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Makes a request towards the services layer for all shifts based on a projectId and date.
        /// used on the 'plan-overzicht' page.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Makes a request towards the services layer for a specific shift based on an id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Makes a request towards the services layer for a shift and all related data to schedule a shift.
        /// Used on the 'plan-shift' page.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                if (numberOfDaysBetweenNowAndStart < 0)
                    numberOfDaysBetweenNowAndStart += 7;
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
                    TaskListResult<Availability> availabilities =
                        await availabilityService.FindAvailabilitiesAsync(
                            availability.Participation.ProjectId,
                            availability.Participation.PersonId);

                    //lookup person information in B2C
                    TaskResult<User> person = await personService.GetUserAsync(availability.Participation.PersonId);

                    //see if person is scheduled this day and this shift
                    if (availabilities.Data == null || availabilities.Data.Count <= 0) continue;
                    int numberOfTimeScheduledThisDay = availabilities.Data
                        .Where(a => a.Shift.Date == shiftResult.Data.Date)
                        .Count(a => a.Type == AvailibilityType.Scheduled);

                    bool scheduledThisShift = availabilities.Data.FirstOrDefault(a =>
                        a.ShiftId == id && a.Type == AvailibilityType.Scheduled) != null;

                    //calculate the number of hours person is scheduled this week
                    double numberOfHoursScheduleThisWeek = availabilities.Data
                        .Where(a => a.Type == AvailibilityType.Scheduled && allDaysThisWeek.Contains(a.Shift.Date))
                        .Sum(availability1 =>
                            availability1.Shift.EndTime.Subtract(availability1.Shift.StartTime).TotalHours);

                    //add scheduleViewmodel to list
                    schedules.Add(new ScheduleViewModel
                    {
                        Person = PersonViewModel.CreateVmFromUser(person.Data,
                            RoosterPlanner.Api.Models.Constants.Extensions.GetInstance(b2CExtentionApplicationId)),
                        NumberOfTimesScheduledThisProject =
                            availabilities.Data.Count(a => a.Type == AvailibilityType.Scheduled),
                        ScheduledThisDay = numberOfTimeScheduledThisDay > 0,
                        ScheduledThisShift = scheduledThisShift,
                        AvailabilityId = availability.Id,
                        Preference = availability.Preference,
                        Availabilities = availabilities.Data
                            .Where(a => a.Shift.Date == shiftResult.Data.Date)
                            .Select(AvailabilityViewModel.CreateVm)
                            .ToList(),
                        HoursScheduledThisWeek = numberOfHoursScheduleThisWeek,
                        Employability = availability.Participation.MaxWorkingHoursPerWeek
                    });
                }

                ScheduleDataViewModel vm = new ScheduleDataViewModel
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

        /// <summary>
        /// Makes a request towards the services layer to save many shifts at once.
        /// Only a Boardmember or a Committeemember can make this request.
        /// </summary>
        /// <param name="shiftViewModels"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpPost]
        public async Task<ActionResult<List<ShiftViewModel>>> SaveShiftsAsync(List<ShiftViewModel> shiftViewModels)
        {
            if (shiftViewModels == null || shiftViewModels.Count == 0)
                return BadRequest("No valid shifts received");

            try
            {
                //filter duplicate dates
                shiftViewModels = shiftViewModels.GroupBy(s => s.Date.Date)
                    .Select(g => g.OrderByDescending(x => x.Date.Date).First()).ToList();

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
                        if (project != null && project.Id != shift.ProjectId)
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

        /// <summary>
        /// Makes a request towards the services layer to update a specific shift.
        /// Only Boardmembers or Committeemembers can make this request.
        /// </summary>
        /// <param name="shiftViewModel"></param>
        /// <returns></returns>
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

                if (!oldShift.RowVersion.SequenceEqual(shiftViewModel.RowVersion))
                    return BadRequest("Outdated entity received");

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
                result.Data.Task.Requirements = null;
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

        /// <summary>
        /// Makes a request towards the services layer to remove a shift.
        /// Only Boardmember or Committeemember can make this request.
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns></returns>
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
                    : Ok(ShiftViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType()
                    .Name + "Error in " + nameof(RemoveShiftAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer for all scheduled shifts and related data.
        /// Used to export data to CSV format.
        /// Only Boardmembers can make this request.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpGet("export/{projectId}")]
        public async Task<ActionResult<List<ShiftViewModel>>> ExportDataAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                TaskListResult<Shift> result = await shiftService.ExportDataAsync(projectId);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null || result.Data.Count == 0)
                    return Ok(new List<ShiftViewModel>());

                List<ShiftViewModel> shiftVmList = new List<ShiftViewModel>();
                List<PersonViewModel> personViewModels = new List<PersonViewModel>();
                foreach (Shift shift in result.Data)
                {
                    ShiftViewModel shiftVm = ShiftViewModel.CreateVm(shift);
                    shiftVm.Availabilities = new List<AvailabilityViewModel>();
                    foreach (Availability availability in shift.Availabilities)
                    {
                        PersonViewModel pvm;
                        Guid id = availability.Participation.PersonId;
                        if (personViewModels.FirstOrDefault(pvms => pvms.Id == id) == null)
                        {
                            TaskResult<User> person = await personService.GetUserAsync(id);
                            pvm = PersonViewModel.CreateVmFromUser(person.Data,
                                RoosterPlanner.Api.Models.Constants.Extensions.GetInstance(b2CExtentionApplicationId));
                            personViewModels.Add(pvm);
                        }
                        else
                            pvm = personViewModels.FirstOrDefault(pvm => pvm.Id == id);

                        AvailabilityViewModel avm = AvailabilityViewModel.CreateVm(availability);
                        avm.Participation.Person = pvm;
                        shiftVm.Availabilities.Add(avm);
                    }

                    shiftVmList.Add(shiftVm);
                }

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
    }
}
