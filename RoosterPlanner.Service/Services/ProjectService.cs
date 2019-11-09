using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IProjectService
    {
        Task<TaskListResult<Project>> GetActiveProjectsAsync();

        Task<TaskListResult<Project>> SearchProjectsAsync(ProjectFilter filter);

        Task<TaskResult<Project>> GetProjectDetails(Guid id);

        TaskResult<Project> CreateProject(Project project);

        TaskResult<Project> UpdateProject(Project project);

        TaskResult<Project> CloseProject(Project project);
    }

    public class ProjectService : IProjectService
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork = null;
        private readonly IProjectRepository projectRepository = null;
        private readonly ILogger logger = null;
        #endregion

        //Constructor
        public ProjectService(IUnitOfWork unitOfWork, ILogger logger)
        {
            this.unitOfWork = unitOfWork;
            this.projectRepository = unitOfWork.ProjectRepository;
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
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het ophalen van actieve projecten.");
                taskResult.Error = ex;
            }
            return taskResult;
        }

        /// <summary>
        /// Search for projects 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<TaskListResult<Project>> SearchProjectsAsync(ProjectFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            TaskListResult<Project> taskResult = TaskListResult<Project>.CreateDefault();

            try
            {
                taskResult.Data = await this.projectRepository.SearchProjectsAsync(filter);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het uitvoeren van een zoekopdracht op projecten.");
                taskResult.Error = ex;
            }
            return taskResult;
        }

        public async Task<TaskResult<Project>> GetProjectDetails(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            TaskResult<Project> taskResult = new TaskResult<Project>();

            try
            {
                taskResult.Data = await this.projectRepository.GetProjectDetails(id);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het uitvoeren van een zoekopdracht op projecten.");
                taskResult.Error = ex;
            }
            return taskResult;
        }

        public TaskResult<Project> CreateProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            TaskResult<Project> taskResult = new TaskResult<Project>();

            try
            {
                taskResult.Data = this.projectRepository.Add(project);
                taskResult.Succeeded = (this.unitOfWork.SaveChanges() == 1);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het uitvoeren van een zoekopdracht op projecten.");
                taskResult.Error = ex;
            }
            return taskResult;
        }

        public TaskResult<Project> UpdateProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            TaskResult<Project> taskResult = new TaskResult<Project>();

            try
            {
                taskResult.Data = this.projectRepository.Update(project);
                taskResult.Succeeded = (this.unitOfWork.SaveChanges() == 1);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het uitvoeren van een zoekopdracht op projecten.");
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
                taskResult.Succeeded = (this.unitOfWork.SaveChanges() == 1);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Fout bij het ophalen van actieve projecten.");
                taskResult.Error = ex;
            }
            return taskResult;
        }
    }
}
