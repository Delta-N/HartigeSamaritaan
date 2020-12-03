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
        private readonly ILogger<ProjectsController> logger;
        private readonly IProjectService projectService;

        //Constructor
        public ProjectsController(IProjectService projectService, ILogger<ProjectsController> logger)
        {
            this.projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDetailsViewModel>> GetProjectAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("No valid id.");

            try
            {
                TaskResult<Project> result = await projectService.GetProjectDetailsAsync(id);

                if (!result.Succeeded)
                    return UnprocessableEntity();
                if (result.Data == null)
                    return NotFound();

                return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity();
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectViewModel>>> SearchAsync(string name,
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
                    return Ok(new List<ProjectViewModel>());

                List<ProjectViewModel> projectVmList = result.Data.Select(ProjectViewModel.CreateVm).ToList();
                return Ok(projectVmList);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity();
            }
        }

        //alleen een bestuurslid kan projecten aanmaken of wijzigen
        [Authorize(Policy = "Boardmember")]
        [HttpPost]
        public async Task<ActionResult<ProjectDetailsViewModel>> SaveProjectAsync(ProjectDetailsViewModel projectDetails)
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

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                project.LastEditBy = oid;

                TaskResult<Project> result;
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
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity();
            }
        }

        [Authorize(Policy = "Boardmember")]
        [HttpPut]
        public async Task<ActionResult<ProjectDetailsViewModel>> UpdateProjectAsync(ProjectDetailsViewModel projectDetails)
        {
            if (projectDetails == null || projectDetails.Id == Guid.Empty)
                return BadRequest("No valid project received");

            if (string.IsNullOrEmpty(projectDetails.Name))
                return BadRequest("Name of project cannot be empty");

            try
            {
                Project oldProject = (await projectService.GetProjectDetailsAsync(projectDetails.Id)).Data;
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
                oldProject.ParticipationStartDate = updatedProject.ParticipationStartDate;
                oldProject.WebsiteUrl = updatedProject.WebsiteUrl;
                oldProject.ProjectStartDate = updatedProject.ProjectStartDate;
                oldProject.ProjectEndDate = updatedProject.ProjectEndDate;

                string oid = IdentityHelper.GetOid(HttpContext.User.Identity as ClaimsIdentity);
                oldProject.LastEditBy = oid;

                TaskResult<Project> result = await projectService.UpdateProjectAsync(oldProject);
                if (!result.Succeeded)
                    return UnprocessableEntity();
                return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.ToString());
                return UnprocessableEntity();
            }
        }
    }
}