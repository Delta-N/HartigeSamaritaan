﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;

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
        public ProjectsController(IProjectService projectService, ILogger logger)
        {
            this.projectService = projectService;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("No valid id.");

            try
            {
                TaskResult<Project> result = await projectService.GetProjectDetails(id);

                if (!result.Succeeded)
                    return UnprocessableEntity();
                return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ProjectController: Error occured.");
                Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectViewModel>>> Search(string name,
            string city,
            DateTime? startDateFrom = null,
            bool? closed = null,
            int offset = 0,
            int pageSize = 20)
        {
            ProjectFilter filter = new ProjectFilter(offset, pageSize)
            {
                Name = name,
                City = city,
                StartDate = startDateFrom,
                Closed = closed
            };

            try
            {
                TaskListResult<Project> result = await projectService.SearchProjectsAsync(filter);
                if (!result.Succeeded)
                    return UnprocessableEntity(result.Message);

                Request.HttpContext.Response.Headers.Add("totalCount", filter.TotalItemCount.ToString());

                List<ProjectViewModel> projectVmList = result.Data.Select(ProjectViewModel.CreateVm)
                    .ToList();
                return Ok(projectVmList);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ProjectController: Error occured.");
                Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        //alleen een bestuurslid kan projecten aanmaken of wijzigen
        [Authorize(Policy = "Boardmember")]
        [HttpPost]
        public ActionResult Save(ProjectDetailsViewModel projectDetails)
        {
            if (projectDetails == null)
                return BadRequest("No valid project received");

            if (string.IsNullOrEmpty(projectDetails.Name))
                return BadRequest("Name of project cannot be empty");

            TaskResult<Project> result = new TaskResult<Project>();

            try
            {
                Project project = ProjectDetailsViewModel.CreateProject(projectDetails);
                project.LastEditDate = DateTime.UtcNow;
                string oid = PersonsController.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                project.LastEditBy = oid ?? null;

                if (project.Id == Guid.Empty)
                    result = projectService.CreateProject(project);
                else if (project.Id != Guid.Empty) result = projectService.UpdateProject(project);

                if (result.Succeeded)
                    return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
                return UnprocessableEntity(projectDetails);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ProjectController: Error occured.");
                Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPatch]
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
                oldProject.EndDate = updatedProject.EndDate;
                oldProject.PictureUri = updatedProject.PictureUri;
                oldProject.ProjectTasks = updatedProject.ProjectTasks;
                oldProject.StartDate = updatedProject.StartDate;
                oldProject.WebsiteUrl = updatedProject.WebsiteUrl;

                oldProject.LastEditDate = DateTime.UtcNow;
                string oid = PersonsController.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldProject.LastEditBy = oid ?? null;
                TaskResult<Project> result = projectService.UpdateProject(oldProject);
                if (!result.Succeeded) return UnprocessableEntity();
                return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ProjectController: Error occured.");
                Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }
    }
}