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
using Shift = RoosterPlanner.Models.Shift;
using Task = RoosterPlanner.Models.Task;

namespace RoosterPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IShiftService shiftService;
        private readonly IProjectService projectService;
        private readonly ITaskService taskService;

        public ShiftController(ILogger<ShiftController> logger, IShiftService shiftService,
            IProjectService projectService, ITaskService taskService)
        {
            this.logger = logger;
            this.shiftService = shiftService;
            this.projectService = projectService;
            this.taskService = taskService;
        }

        [HttpGet("project/{id}")]
        public async Task<ActionResult> GetShifts(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id");
            try
            {
                TaskListResult<Shift> result = await shiftService.GetShifts(id);
                if (!result.Succeeded)
                    return UnprocessableEntity();
                if (result.Data == null || result.Data.Count()==0)
                    return Ok();
                List<ShiftViewModel> shiftVmList = result.Data.Select(ShiftViewModel.CreateVm).ToList();
                return Ok(shiftVmList);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [HttpGet("shift/{id}")]
        public async Task<ActionResult> GetShift(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();
            try
            {
                TaskResult<Shift> result = await shiftService.GetShift(id);
                if (!result.Succeeded)
                    return UnprocessableEntity();
                if (result.Data == null)
                    return Ok();
                return Ok(ShiftViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpPost]
        public async Task<ActionResult> Save(List<ShiftViewModel> shiftViewModels)
        {
            if (shiftViewModels == null || shiftViewModels.Count == 0)
                return BadRequest("No valid shifts received");

            try
            {
                List<Shift> shifts = shiftViewModels.Select(ShiftViewModel.CreateShift).ToList();
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);

                //get project and get task from db
                Project project = projectService.GetProjectDetails(shifts[0].ProjectId).Result.Data;
                Task task = null;
                if (shifts[0].TaskId != null)
                    task = taskService.GetTask((Guid) shifts[0].TaskId).Result.Data;

                foreach (Shift shift in shifts)
                {
                    if (shift.Id != Guid.Empty || shift.TaskId == null || shift.ProjectId == Guid.Empty)
                        shifts.Remove(shift);
                    
                    // check if projectId and taskId differs from above? getproject/task => add project and task to shift
                    if(project.Id!=shift.ProjectId)
                        project=projectService.GetProjectDetails(shift.ProjectId).Result.Data;
                    
                    if(task !=null && shift.TaskId!=null &&  task.Id!=shift.TaskId)
                        task = taskService.GetTask((Guid) shift.TaskId).Result.Data;

                    if (project == null || task == null)
                        shifts.Remove(shift);
                    shift.Project = project;
                    shift.Task = task;
                    shift.LastEditDate = DateTime.UtcNow;
                    shift.LastEditBy = oid;
                }

                if (shifts.Count() != shiftViewModels.Count())
                    return UnprocessableEntity("Could not covert al viewmodels to shifts");

                TaskListResult<Shift> result = await shiftService.CreateShifts(shifts);

                if (result.Succeeded)
                {
                    List<ShiftViewModel> createdVm = result.Data.Select(ShiftViewModel.CreateVm).ToList();
                    return Ok(createdVm);
                }

                return UnprocessableEntity();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpPut]
        public async Task<ActionResult> Update(ShiftViewModel shiftViewModel)
        {
            if (shiftViewModel == null || shiftViewModel.Id == Guid.Empty || shiftViewModel.Project == null ||
                shiftViewModel.Task == null)
                return BadRequest("No valid Shift received");
            if (shiftViewModel.Task.Id == Guid.Empty || shiftViewModel.Project.Id == Guid.Empty)
                return BadRequest("ProjectId and or TaskId cannot be empty");
            try
            {
                Shift oldShift = shiftService.GetShift(shiftViewModel.Id).Result.Data;
                if (oldShift == null)
                    return NotFound("Shift not found");

                if (oldShift.ProjectId != shiftViewModel.Project.Id)
                    return BadRequest("Cannot change the project of a shift");

                Shift updatedShift = ShiftViewModel.CreateShift(shiftViewModel);
                if (updatedShift == null)
                    return BadRequest("Unable to convert ShiftViewModel to Shift");

                Project project = projectService.GetProjectDetails(updatedShift.ProjectId).Result.Data;
                Task task = null;
                if (updatedShift.TaskId != null)
                    task = taskService.GetTask((Guid) updatedShift.TaskId).Result.Data;

                if (project == null || task == null)
                    return NotFound("Project and or Task Not found");

                oldShift.StartTime = updatedShift.StartTime;
                oldShift.EndTime = updatedShift.EndTime;
                oldShift.ParticipantsRequired = updatedShift.ParticipantsRequired;
                oldShift.Task = task;
                oldShift.Project = project;
                //cannot update Project, Id or Date by design
                oldShift.LastEditDate = DateTime.UtcNow;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldShift.LastEditBy = oid;

                TaskResult<Shift> result = await shiftService.UpdateShift(oldShift);

                if (!result.Succeeded)
                    return UnprocessableEntity();
                return Ok(ShiftViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember&Committeemember")]
        [HttpDelete]
        public async Task<ActionResult> RemoveShift(Guid shiftId)
        {
            if (shiftId == Guid.Empty)
                return BadRequest("No valid id.");
            try
            {
                Task<TaskResult<Shift>> shift = shiftService.GetShift(shiftId);
                if (shift == null)
                    return NotFound("Shift not found");

                TaskResult<Shift> result = await shiftService.RemoveShift(shift.Result.Data);
                if (!result.Succeeded)
                    return Problem();
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }
    }
}