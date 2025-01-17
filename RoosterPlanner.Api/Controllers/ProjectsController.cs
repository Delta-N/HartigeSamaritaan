﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Models.EntityViewModels;
using RoosterPlanner.Api.Models.HelperViewModels;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.Helpers;
using RoosterPlanner.Service.Services;
using Type = RoosterPlanner.Api.Models.HelperViewModels.Type;

namespace RoosterPlanner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> logger;
        private readonly IProjectService projectService;

        //Constructor
        public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
        {
            this.projectService = projectService;
            this.logger = logger;
        }
        /// <summary>
        /// Makes a request towards the services layer to get a project based on a Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetailsViewModel>> GetProjectAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");

            try
            {
                TaskResult<Project> result = await projectService.GetProjectDetailsAsync(id);

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                if (result.Data == null)
                    return NotFound();

                return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(GetProjectAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
        /// <summary>
        /// /// Makes a request towards the services layer for a project based on attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="city"></param>
        /// <param name="startDateFrom"></param>
        /// <param name="endDate"></param>
        /// <param name="closed"></param>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<ProjectViewModel>>> SearchProjectsAsync(string name,
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
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});

                if (result.Data == null)
                    return Ok(new List<ProjectViewModel>());

                List<ProjectViewModel> projectVmList = result.Data.Select(ProjectViewModel.CreateVm).ToList();

                return Ok(new SearchResultViewModel<ProjectViewModel>(filter.TotalItemCount, projectVmList));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SearchProjectsAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }

        /// <summary>
        /// Makes a request towards the services layer to add a project to the database.
        /// Only Boardmembers can add projects.
        /// </summary>
        /// <param name="projectDetails"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpPost]
        public async Task<ActionResult<ProjectDetailsViewModel>> SaveProjectAsync(
            ProjectDetailsViewModel projectDetails)
        {
            if (projectDetails == null)
                return BadRequest("No valid project received");

            if (string.IsNullOrEmpty(projectDetails.Name))
                return BadRequest("Name of project cannot be empty");

            try
            {
                Project project = ProjectDetailsViewModel.CreateProject(projectDetails);
                if (project == null)
                    return BadRequest("Unable to convert ProjectDetailsViewmodel to Project");
                project.PictureUri = null;

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                project.LastEditBy = oid;

                TaskResult<Project> result;
                if (project.Id == Guid.Empty)
                    result = await projectService.CreateProjectAsync(project);
                else
                    return BadRequest("Cannot update existing Project with post method");

                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(SaveProjectAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
        /// <summary>
        /// Makes a request towards the services layer to update a project.
        /// Only Boardmembers can update a project.
        /// </summary>
        /// <param name="projectDetails"></param>
        /// <returns></returns>
        [Authorize(Policy = "Boardmember")]
        [HttpPut]
        public async Task<ActionResult<ProjectDetailsViewModel>> UpdateProjectAsync(
            ProjectDetailsViewModel projectDetails)
        {
            if (projectDetails == null || projectDetails.Id == Guid.Empty)
                return BadRequest("No valid project received");

            if (string.IsNullOrEmpty(projectDetails.Name))
                return BadRequest("Name of project cannot be empty");

            try
            {
                Project oldProject = (await projectService.GetProjectDetailsAsync(projectDetails.Id)).Data;
                
                if (!oldProject.RowVersion.SequenceEqual(projectDetails.RowVersion))
                    return BadRequest("Outdated entity received");
                
                Project updatedProject = ProjectDetailsViewModel.CreateProject(projectDetails);
                oldProject.Address = updatedProject.Address;
                oldProject.City = updatedProject.City;
                oldProject.Closed = updatedProject.Closed;
                oldProject.Description = updatedProject.Description;
                oldProject.Name = updatedProject.Name;
                oldProject.Participations = updatedProject.Participations;
                oldProject.Shifts = updatedProject.Shifts;
                oldProject.ParticipationEndDate = updatedProject.ParticipationEndDate;
                oldProject.PictureUriId = updatedProject.PictureUriId;
                oldProject.ParticipationStartDate = updatedProject.ParticipationStartDate;
                oldProject.WebsiteUrl = updatedProject.WebsiteUrl;
                oldProject.ProjectStartDate = updatedProject.ProjectStartDate;
                oldProject.ProjectEndDate = updatedProject.ProjectEndDate;
                oldProject.ContactAdres = updatedProject.ContactAdres;

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldProject.LastEditBy = oid;

                TaskResult<Project> result = await projectService.UpdateProjectAsync(oldProject);
                if (!result.Succeeded)
                    return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = result.Message});
                return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                string message = GetType().Name + "Error in " + nameof(UpdateProjectAsync);
                logger.LogError(ex, message);
                return UnprocessableEntity(new ErrorViewModel {Type = Type.Error, Message = message});
            }
        }
    }
}