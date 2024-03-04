using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Repositories;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Service.DataModels;
namespace RoosterPlanner.Service.Services
{
    public interface IProjectService
    {
        /// <summary>
        /// Makes a call to the repository layer and requests projects based on a filter.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<TaskListResult<Project>> SearchProjectsAsync(ProjectFilter filter);

        /// <summary>
        /// Makes a call to the repository layer and requests a project based on a id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TaskResult<Project>> GetProjectDetailsAsync(Guid id);

        /// <summary>
        /// Makes a call to the repository layer and requests a addition of a project.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        Task<TaskResult<Project>> CreateProjectAsync(Project project);

        /// <summary>
        /// Makes a call to the repository layer and requests a addition of a project.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        Task<TaskResult<Project>> UpdateProjectAsync(Project project);
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
        /// Makes a call to the repository layer and requests projects based on a filter.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<TaskListResult<Project>> SearchProjectsAsync(ProjectFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            TaskListResult<Project> result = TaskListResult<Project>.CreateDefault();

            try
            {
                result.Data = await projectRepository.SearchProjectsAsync(filter);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error finding projects with filter: " + filter;
                logger.LogError(ex, result.Message, filter);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests a project based on a id.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TaskResult<Project>> GetProjectDetailsAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;

            TaskResult<Project> result = new TaskResult<Project>();

            try
            {
                result.Data = await projectRepository.GetProjectDetailsAsync(id);
                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error getting project details " + id;
                logger.LogError(ex, result.Message);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests a addition of a project.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task<TaskResult<Project>> CreateProjectAsync(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            TaskResult<Project> result = new TaskResult<Project>();

            try
            {
                result.Data = projectRepository.Add(project);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error creating project " + project.Id;
                logger.LogError(ex, result.Message, project);
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Makes a call to the repository layer and requests a update of a project.
        /// Wraps the result of this request in a TaskResult wrapper.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task<TaskResult<Project>> UpdateProjectAsync(Project project)
        {
            if (project == null || project.Id == Guid.Empty)
                throw new ArgumentNullException(nameof(project));

            TaskResult<Project> result = new TaskResult<Project>();

            try
            {
                result.Data = projectRepository.Update(project);
                result.Succeeded = await unitOfWork.SaveChangesAsync() == 1;
            }
            catch (Exception ex)
            {
                result.Message = GetType().Name + " - Error updating project " + project.Id;
                logger.LogError(ex, result.Message, project);
                result.Error = ex;
            }

            return result;
        }
    }
}