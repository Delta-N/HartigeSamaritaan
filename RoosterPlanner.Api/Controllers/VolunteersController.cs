using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Service;

namespace RoosterPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteersController : ControllerBase
    {
        protected IMapper Mapper { get; set; }
        public IProjectService ProjectService { get; set; }

        public VolunteersController(IMapper mapper, IProjectService projectService)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            ProjectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        }

        [HttpGet("getprojects")]
        public async Task<ActionResult<IEnumerable<Projects>>> Get()
        {
            var projects = await ProjectService.GetActiveProjectsAsync();
            return  projects.Data.Select(i => Mapper.Map<Projects>(i)).ToList();
        }
    }
}