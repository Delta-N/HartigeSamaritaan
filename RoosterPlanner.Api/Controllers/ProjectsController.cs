﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IProjectService projectService;

        //Constructor
        public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
        {
            this.projectService = projectService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetailsViewModel>> GetProjectAsync(Guid id)
        {
            if (id == Guid.Empty) 
                return BadRequest("No valid id.");

            try
            {
                TaskResult<Project> result = await projectService.GetProjectDetails(id);

                if (!result.Succeeded)
                    return UnprocessableEntity();
                if (result.Data == null)
                    return Ok();
                
                return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetProjectAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectViewModel>>> Search(string name,
            string city,
            DateTime? startDateFrom = null,
            DateTime? endDate = null,
            bool? closed = null,
            int offset = 0,
            int pageSize = 20)
        {
            ProjectFilter filter = new ProjectFilter(offset, pageSize)
            {
                Name = name,
                City = city,
                StartDate = startDateFrom,
                EndDate = endDate,
                Closed = closed
            };

            try
            {
                TaskListResult<Project> result = await projectService.SearchProjectsAsync(filter);
                if (!result.Succeeded)
                    return UnprocessableEntity(result.Message);

                Request.HttpContext.Response.Headers.Add("totalCount", filter.TotalItemCount.ToString());
                if (result.Data == null)
                    return Ok();

                List<ProjectViewModel> projectVmList = result.Data.Select(ProjectViewModel.CreateVm).ToList();
                return Ok(projectVmList);
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SearchProjectsAsync);
                logger.LogError(ex,message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        //alleen een bestuurslid kan projecten aanmaken of wijzigen
        [Authorize(Policy = "Boardmember")]
        [HttpPost]
        public async Task<ActionResult<ProjectDetailsViewModel>> SaveProjectAsync(
            ProjectDetailsViewModel projectDetails)
        {
            if (projectDetails == null)
                return BadRequest("No valid project received");

            if (string.IsNullOrEmpty(projectDetails.Name))
                return BadRequest("Name of project cannot be empty");

            TaskResult<Project> result;

            try
            {
                Project project = ProjectDetailsViewModel.CreateProject(projectDetails);
                if (project == null)
                    return BadRequest("Unable to convert ProjectDetailsViewmodel to Project");
                
                project.LastEditDate = DateTime.UtcNow;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                project.LastEditBy = oid;

                if (project.Id == Guid.Empty)
                    result = await projectService.CreateProjectAsync(project);
                else
                    return BadRequest("Cannot update existing Project with post method");

                if (result.Succeeded)
                    return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
                return UnprocessableEntity(projectDetails);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPut]
        public ActionResult UpdateProject(ProjectDetailsViewModel projectDetails)
        {
            if (projectDetails == null || projectDetails.Id == Guid.Empty)
                return BadRequest("No valid project received");

            if (string.IsNullOrEmpty(projectDetails.Name))
                return BadRequest("Name of project cannot be empty");

            try
            {
                Project oldProject = projectService.GetProjectDetails(projectDetails.Id)
                    .Result.Data;
                Project updatedProject = ProjectDetailsViewModel.CreateProject(projectDetails);
                oldProject.Address = updatedProject.Address;
                oldProject.City = updatedProject.City;
                oldProject.Closed = updatedProject.Closed;
                oldProject.Description = updatedProject.Description;
                oldProject.Name = updatedProject.Name;
                oldProject.Participations = updatedProject.Participations;
                oldProject.Shifts = updatedProject.Shifts;
                oldProject.ParticipationEndDate = updatedProject.ParticipationEndDate;
                oldProject.PictureUri = updatedProject.PictureUri;
                oldProject.ProjectTasks = updatedProject.ProjectTasks;
                oldProject.ParticipationStartDate = updatedProject.ParticipationStartDate;
                oldProject.WebsiteUrl = updatedProject.WebsiteUrl;
                oldProject.ProjectStartDate = updatedProject.ProjectStartDate;
                oldProject.ProjectEndDate = updatedProject.ProjectEndDate;

                oldProject.LastEditDate = DateTime.UtcNow;
                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldProject.LastEditBy = oid;
                
                TaskResult<Project> result = projectService.UpdateProject(oldProject).Result;
                if (!result.Succeeded) 
                    return UnprocessableEntity();
                return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,ex.ToString());
                Response.Headers.Add("message", ex.Message);
                return UnprocessableEntity();
            }
        }
    }
}