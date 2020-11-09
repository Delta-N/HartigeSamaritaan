using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Api.Models;
using RoosterPlanner.Common;
using RoosterPlanner.Service;
using RoosterPlanner.Service.DataModels;
using Task = RoosterPlanner.Models.Task;

namespace RoosterPlanner.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITaskService taskService;
        private readonly ILogger logger;

        //Constructor
        public TasksController(IMapper mapper, ITaskService taskService, ILogger<TasksController> logger)
        {
            this.mapper = mapper;
            this.taskService = taskService;
            this.logger = logger;
        }

    }
}