using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IProjectService projectService;
        private readonly ILogger logger;

        //Constructor
        public ProjectsController(IProjectService projectService, ILogger logger)
        {
            this.projectService = projectService;
            this.logger = logger;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            ProjectDetailsViewModel projectDetailsVm;

            try
            {
                if (id != Guid.Empty)
                {
                    TaskResult<Project> result = await this.projectService.GetProjectDetails(id);
                    if (result.Succeeded)
                    {
                        projectDetailsVm = ProjectDetailsViewModel.CreateVm(result.Data);
                        return Ok(projectDetailsVm);
                    }

                    return UnprocessableEntity();
                }

                return BadRequest("No valid id.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ProjectController: Error occured.");
                this.Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        [HttpGet()]
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

            List<ProjectViewModel> projectVmList;

            try
            {
                TaskListResult<Project> result = await this.projectService.SearchProjectsAsync(filter);
                if (result.Succeeded)
                {
                    Request.HttpContext.Response.Headers.Add("totalCount", filter.TotalItemCount.ToString());
                    projectVmList = result.Data.Select(ProjectViewModel.CreateVm)
                        .ToList();
                    return Ok(projectVmList);
                }
                return UnprocessableEntity(result.Message);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ProjectController: Error occured.");
                this.Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        //alleen een bestuurslid kan projecten aanmaken of wijzigen
        [Authorize(Policy = "Boardmember")]
        [HttpPost()]
        public ActionResult Save(ProjectDetailsViewModel projectDetails)
        {
            if (projectDetails == null)
                return BadRequest("Er is geen geldig project ontvangen.");

            if (String.IsNullOrEmpty(projectDetails.Name))
                return BadRequest("De naam van het project mag niet leeg zijn.");

            TaskResult<Project> result = new TaskResult<Project>();

            try
            {
                Project project = ProjectDetailsViewModel.CreateProject(projectDetails);

                if (project != null && project.Id == Guid.Empty)
                {
                    result = this.projectService.CreateProject(project);
                }
                else if (project != null && project.Id != Guid.Empty)
                {
                    result = this.projectService.UpdateProject(project);
                }

                if (result.Succeeded)
                    return Ok(ProjectDetailsViewModel.CreateVm(result.Data));
                return UnprocessableEntity(projectDetails);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "ProjectController: Error occured.");
                this.Response.Headers.Add("message", ex.Message);
            }

            return NoContent();
        }

        [HttpPost("{id}/addperson/{personId}")]
        public ActionResult AddPerson(Guid id, Guid personId)
        {
            if (id == Guid.Empty)
                return BadRequest("id");

            if (this.projectService.AddPersonToProject(id, personId) == 1)
                return Ok();
            else
                return UnprocessableEntity();
        }
    }
}