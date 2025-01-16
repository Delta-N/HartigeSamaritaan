using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IProjectTaskRepository : IRepository<ProjectTask>
    {
        /// <summary>
        /// Get all projectTasks in database.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A task of all projectTasks in database.</returns>
        Task<List<ProjectTask>> GetAllFromProjectAsync(Guid projectId);
        /// <summary>
        /// Get a projectTask based on projectId and taskId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="taskId"></param>
        /// <returns>A task of a projectTask</returns>
        Task<ProjectTask> GetProjectTaskAsync(Guid projectId, Guid taskId);
    }

    public class ProjectTaskRepository : Repository<ProjectTask>, IProjectTaskRepository
    {
        public ProjectTaskRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Get all projectTasks in database.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A task of all projectTasks in database.</returns>
        public async Task<List<ProjectTask>> GetAllFromProjectAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return null;
            List<ProjectTask> projectTasks= await EntitySet
                .AsNoTracking()
                .Include(pt => pt.Project)
                .Include(pt => pt.Task)
                .ThenInclude(t=>t.Requirements)
                .Where(pt => pt.ProjectId == projectId).ToListAsync();

            foreach (Requirement requirement in projectTasks.SelectMany(projectTask => projectTask.Task.Requirements))
            {
                requirement.Task = null;
                requirement.CertificateType = null;
            }
            
            return projectTasks;
        }

        /// <summary>
        /// Get a projectTask based on projectId and taskId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="taskId"></param>
        /// <returns>A task of a projectTask</returns>
        public Task<ProjectTask> GetProjectTaskAsync(Guid projectId, Guid taskId)
        {
            if (projectId == Guid.Empty || taskId == Guid.Empty)
                return null;
            return  EntitySet
                .AsNoTracking()
                .Where(pt => pt.ProjectId == projectId && pt.TaskId == taskId)
                .FirstOrDefaultAsync();
        }
    }
}