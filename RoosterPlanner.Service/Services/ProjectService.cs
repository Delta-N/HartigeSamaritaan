﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IProjectService
    {
        Task<TaskListResult<Project>> GetActiveProjectsAsync();

        TaskResult<Project> CloseProject(Project project);
    }

    public class ProjectService : IProjectService
    {
        #region Fields
        private readonly IProjectRepository projectRepository = null;
        private readonly ILogger logger = null;
        #endregion

        private readonly Data.Context.RoosterPlannerContext dataContext = null;

        //Constructor
        public ProjectService(Data.Context.RoosterPlannerContext dataContext, IProjectRepository projectRepository, ILogger logger)
        {
            this.dataContext = dataContext;
            this.projectRepository = projectRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public async Task<TaskListResult<Project>> GetActiveProjectsAsync()
        {
            TaskListResult<Project> taskResult = TaskListResult<Project>.CreateDefault();

            try
            {
                taskResult.Data = await this.projectRepository.GetActiveProjectsAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij het ophalen van actieve projecten.");
                taskResult.Error = ex;
            }
            return taskResult;
        }

        /// <summary>
        /// Closes the project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public TaskResult<Project> CloseProject(Project project)
        {
            TaskResult<Project> taskResult = new TaskResult<Project>();

            try
            {
                project.Closed = true;
                taskResult.Data = this.projectRepository.AddOrUpdate(project);
                taskResult.Succeeded = (this.dataContext.SaveChanges() == 1);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fout bij het ophalen van actieve projecten.");
                taskResult.Error = ex;
            }
            return taskResult;
        }
    }
}
