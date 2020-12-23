﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Api.Models.Constants;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Models.Enums;
using RoosterPlanner.Models.Types;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using Type = RoosterPlanner.Api.Models.Type;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly ILogger<AvailabilityController> logger;
        private readonly IAvailabilityService availabilityService;
        private readonly IShiftService shiftService;
        private readonly ITaskService taskService;

        public AvailabilityController(
            ILogger<AvailabilityController> logger,
            IAvailabilityService availabilityService,
            IShiftService shiftService,
            ITaskService taskService)
        {
            this.logger = logger;
            this.availabilityService = availabilityService;
            this.shiftService = shiftService;
            this.taskService = taskService;
        }

        [HttpGet("find/{projectId}/{userId}")]
        public async Task<ActionResult<AvailabilityDataViewModel>> GetAvailabilityData(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid projectId.");
            if (userId == Guid.Empty)
                return BadRequest("No valid userId.");
            try
            {
                TaskListResult<ProjectTask> taskResult = await taskService.GetAllProjectTasksAsync(projectId);
                TaskListResult<Shift> shiftResult =
                    await shiftService.GetShiftsWithAvailabilitiesAsync(projectId, userId);

                if (!taskResult.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = taskResult.Message});
                if (!shiftResult.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = shiftResult.Message});

                List<Schedule> knownAvailabilities = new List<Schedule>();

                foreach (IGrouping<DateTime, Shift> grouping in shiftResult.Data.GroupBy(s => s.Date))
                {
                    int numberOfShifts = grouping.Count();
                    int numberOfAvailabilities = 0;
                    bool scheduled = false;
                    foreach (Shift shift in grouping)
                    {
                        if (shift.Availabilities.Count <= 0) continue;
                        numberOfAvailabilities++;
                        shift.Availabilities.ForEach(a =>
                        {
                            if (a.Type == AvailibilityType.Scheduled)
                                scheduled = true;
                        });
                    }

                    if (scheduled)
                        knownAvailabilities.Add(new Schedule(grouping.Key, AvailabilityStatus.Scheduled));
                    else if (numberOfShifts == numberOfAvailabilities)
                        knownAvailabilities.Add(new Schedule(grouping.Key, AvailabilityStatus.Complete));
                    else
                        knownAvailabilities.Add(new Schedule(grouping.Key, AvailabilityStatus.Incomplete));
                }

                List<TaskViewModel> taskViewModels = taskResult.Data
                    .Select(projectTask => TaskViewModel.CreateVm(projectTask.Task)).ToList();

                AvailabilityDataViewModel vm = new AvailabilityDataViewModel
                {
                    ProjectTasks = taskViewModels,
                    KnownAvailabilities = knownAvailabilities
                };
                return Ok(vm);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetAvailabilityData);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpGet("find/{projectId}")]
        public async Task<ActionResult<AvailabilityDataViewModel>> GetAvailabilityData(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return BadRequest("No valid projectId.");
            try
            {
                TaskListResult<ProjectTask> taskResult = await taskService.GetAllProjectTasksAsync(projectId);
                TaskListResult<Shift> shiftResult =
                    await shiftService.GetShiftsWithAvailabilitiesAsync(projectId);

                if (!taskResult.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = taskResult.Message});
                if (!shiftResult.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel
                        {Type = Type.Error, Message = shiftResult.Message});

                List<Schedule> knownAvailabilities = new List<Schedule>();

                foreach (IGrouping<DateTime, Shift> grouping in shiftResult.Data.GroupBy(s => s.Date))
                {
                    List<AvailabilityStatus> dateStatus = new List<AvailabilityStatus>();
                    foreach (Shift shift in grouping)
                    {
                        int numberOfAvailabilities = shift.Availabilities.Count;
                        int numberOfSchedule = shift.Availabilities.Count(a => a.Type == AvailibilityType.Scheduled);
                        if (numberOfSchedule >= shift.ParticipantsRequired)
                            dateStatus.Add(AvailabilityStatus.Scheduled);
                        else if (numberOfAvailabilities >= shift.ParticipantsRequired)
                            dateStatus.Add(AvailabilityStatus.Complete);
                        else
                            dateStatus.Add(AvailabilityStatus.Incomplete);
                    }

                    Schedule schedule = new Schedule(grouping.Key, AvailabilityStatus.Incomplete);
                    if (dateStatus.All(a => a == AvailabilityStatus.Complete || a == AvailabilityStatus.Scheduled))
                        schedule.Status = AvailabilityStatus.Complete;
                    if (dateStatus.All(a => a == AvailabilityStatus.Scheduled))
                        schedule.Status = AvailabilityStatus.Scheduled;

                    knownAvailabilities.Add(schedule);
                }

                List<TaskViewModel> taskViewModels = taskResult.Data
                    .Select(projectTask => TaskViewModel.CreateVm(projectTask.Task)).ToList();

                AvailabilityDataViewModel vm = new AvailabilityDataViewModel
                {
                    ProjectTasks = taskViewModels,
                    KnownAvailabilities = knownAvailabilities
                };
                return Ok(vm);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetAvailabilityData);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpPost]
        public async Task<ActionResult<Availability>> SaveAvailabilityAsync(AvailabilityViewModel availabilityViewModel)
        {
            if (availabilityViewModel == null)
                return BadRequest("No valid availability received");
            if (availabilityViewModel.ShiftId == Guid.Empty)
                return BadRequest("No valid shiftId received");
            if (availabilityViewModel.ParticipationId == Guid.Empty)
                return BadRequest("No valid participationId received");

            try
            {
                Availability availability = AvailabilityViewModel.CreateAvailability(availabilityViewModel);
                if (availability == null)
                    return BadRequest("Unable to convert availabilityViewModel to Availability");

                if (availability.Participation == null || availability.Shift == null)
                    return BadRequest("Unable to add Participation and/of shift to Availability");

                availability.Participation = null;
                availability.Shift = null;

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                availability.LastEditBy = oid;

                TaskResult<Availability> result;
                if (availability.Id == Guid.Empty)
                    result = await availabilityService.AddAvailability(availability);
                else
                    return BadRequest("Cannot update existing Availability with post method");

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(AvailabilityViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SaveAvailabilityAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpPut]
        public async Task<ActionResult<AvailabilityViewModel>> UpdateAvailabilityAsync(
            AvailabilityViewModel availabilityViewModel)
        {
            if (availabilityViewModel == null || availabilityViewModel.Id == Guid.Empty)
                return BadRequest("No valid availability received");
            if (availabilityViewModel.ShiftId == Guid.Empty)
                return BadRequest("No valid shiftId received");
            if (availabilityViewModel.ParticipationId == Guid.Empty)
                return BadRequest("No valid participationId received");
            try
            {
                Availability availability = (await availabilityService.GetAvailability(availabilityViewModel.Id)).Data;
                if (availability == null)
                    return BadRequest("Unable to convert availabilityViewModel to Availability");
                if (availability.Type == AvailibilityType.Scheduled)
                    return BadRequest("Cannot modify availability when user is already scheduled");

                availability.Preference = availabilityViewModel.Preference;
                availability.Type = availabilityViewModel.Type;

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                availability.LastEditBy = oid;

                TaskResult<Availability> result = await availabilityService.UpdateAvailability(availability);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                AvailabilityViewModel vm = AvailabilityViewModel.CreateVm(result.Data);

                return Ok(vm);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SaveAvailabilityAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpPatch]
        public async Task<ActionResult<bool>> UpdateAvailabilities(List<ScheduleViewModel> scheduleViewModels)
        {
            if (scheduleViewModels == null)
                return BadRequest("No Schedules received");
            bool invalidSchedule = false;
            scheduleViewModels.ForEach(vm =>
            {
                if (vm.AvailabilityId == Guid.Empty || vm.Person == null || vm.Person.Id == Guid.Empty)
                    invalidSchedule = true;
            });
            if (invalidSchedule)
                return BadRequest("Invalid Schedules received");

            try
            {
                bool succeeded = true;
                string message = "Error adding: ";
                foreach (ScheduleViewModel scheduleViewModel in scheduleViewModels)
                {
                    Availability availability =
                        (await availabilityService.GetAvailability(scheduleViewModel.AvailabilityId)).Data;

                    if (availability == null) continue;
                    availability.Type = scheduleViewModel.ScheduledThisDay
                        ? AvailibilityType.Scheduled
                        : AvailibilityType.Ok;
                    TaskResult<Availability> result = await availabilityService.UpdateAvailability(availability);

                    if (result.Succeeded) continue;
                    succeeded = false;
                    message += scheduleViewModel.AvailabilityId;
                }

                if (!succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});

                return Ok(true);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SaveAvailabilityAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
    }
}