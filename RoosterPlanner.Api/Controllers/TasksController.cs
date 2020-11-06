﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        public TasksController(IMapper mapper, ITaskService taskService, ILogger logger)
        {
            this.mapper = mapper;
            this.taskService = taskService;
            this.logger = logger;
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetActiveTasks()
        {
            TaskListResult<Task> taskListResult = await this.taskService.GetActiveTasksAsync();
            if (taskListResult.Succeeded)
            {
                return Ok(taskListResult.Data.Select(t => mapper.Map<TaskViewModel>(t)).ToList());
            }
            return UnprocessableEntity();
        }

        [HttpDelete()]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            if (id != Guid.Empty)
            {
                TaskResult result = await this.taskService.SetTaskDeleteAsync(id);
                if (result.Succeeded)
                    return Ok();
                else
                    return UnprocessableEntity(result.Message);
            }
            return BadRequest("No valid id.");
        }
    }
}