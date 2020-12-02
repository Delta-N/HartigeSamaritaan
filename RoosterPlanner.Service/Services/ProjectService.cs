using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service.DataModels;

namespace RoosterPlanner.Service
{
    public interface IProjectService
    {
        Task<TaskListResult<Project>> SearchProjectsAsync(ProjectFilter filter);

        Task<TaskResult<Project>> GetProjectDetails(Guid id);

        Task<TaskResult<Project>> CreateProject(Project project);

        Task<TaskResult<Project>> UpdateProject(Project project);
    }

    public class ProjectService : IProjectService
    {
        #region Fields
        private readonly IUnitOfWork unitOfWork;
        private readonly IProjectRepository projectRepository;
        private readonly ILogger<ProjectService> logger;
        #endregion

        //Constructor
        public ProjectService(IUnitOfWork unitOfWork, ILogger<ProjectService> logger)
        {
            this.unitOfWork = unitOfWork;
            projectRepository = unitOfWork.ProjectRepository;
            this.logger = logger;
        }
        
        /// <summary>
        /// Search for projects 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<TaskListResult<Project>> SearchProjectsAsync(ProjectFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            TaskListResult<Project> taskResult = TaskListResult<Project>.CreateDefault();

            try
            {
                taskResult.Data = await projectRepository.SearchProjectsAsync(filter);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,ex.ToString());
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
                taskResult.Data = await projectRepository.GetProjectDetails(id);
                taskResult.Succeeded = true;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,ex.ToString());
                taskResult.Error = ex;
            }
            return taskResult;
        }

        public async Task<TaskResult<Project>> CreateProject(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            TaskResult<Project> taskResult = new TaskResult<Project>();

            try
            {
                taskResult.Data = projectRepository.Add(project);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,ex.ToString());
                taskResult.Error = ex;
            }
            return taskResult;
        }

        public async Task<TaskResult<Project>> UpdateProject(Project project)
        {
            if (project == null || project.Id==Guid.Empty)
                throw new ArgumentNullException(nameof(project));

            TaskResult<Project> taskResult = new TaskResult<Project>();

            try
            {
                taskResult.Data = projectRepository.Update(project);
                taskResult.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error,ex.ToString());
                taskResult.Error = ex;
            }
            return taskResult;
        }
    }
}
