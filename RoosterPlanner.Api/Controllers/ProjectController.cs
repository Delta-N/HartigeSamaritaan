using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ProjectController : ControllerBase
    {
        private readonly IMapper mapper = null;
        private readonly IProjectService projectService = null;
        private readonly ILogger logger = null;

        //Constructor
        public ProjectController(IMapper mapper, IProjectService projectService, ILogger logger)
        {
            this.mapper = mapper;
            this.projectService = projectService;
            this.logger = logger;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            ProjectDetailsViewModel projectDetailsVm = new ProjectDetailsViewModel();

            try
            {
                TaskResult<Project> result = await this.projectService.GetProjectDetails(id);
                if (result.Succeeded)
                {
                    projectDetailsVm = ProjectDetailsViewModel.CreateVm(result.Data);
                }

                return Ok(projectDetailsVm);
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
            ProjectFilter filter = new ProjectFilter(offset, pageSize);
            filter.Name = name;
            filter.City = city;
            filter.StartDate = startDateFrom;
            filter.Closed = closed;

            List<ProjectViewModel> projectVmList = new List<ProjectViewModel>();

            try
            {
                TaskListResult<Project> result = await this.projectService.SearchProjectsAsync(filter);
                if (result.Succeeded)
                {
                    Request.HttpContext.Response.Headers.Add("totalCount", filter.TotalItemCount.ToString());
                    projectVmList = result.Data.Select(ProjectViewModel.CreateVm).ToList();
                    return Ok(projectVmList);
                }
                else
                {
                    return UnprocessableEntity(result.Message);
                }
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
                return BadRequest("De projectnaam mag niet leeg zijn.");

            TaskResult<Project> result = new TaskResult<Project>();

            try
            {
                Project project = mapper.Map<Project>(projectDetails);
                if (project != null && project.Id == Guid.Empty)
                {
                    result = this.projectService.CreateProject(project);
                }
                else if (project.Id != Guid.Empty)
                {
                    result = this.projectService.UpdateProject(project);
                }

                if (result.Succeeded)
                    return Ok(this.mapper.Map<ProjectDetailsViewModel>(result.Data));
                else
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