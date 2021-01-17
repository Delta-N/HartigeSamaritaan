using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IProjectTaskRepository : IRepository<ProjectTask>
    {
        Task<List<ProjectTask>> GetAllFromProjectAsync(Guid projectId);
        Task<ProjectTask> GetProjectTaskAsync(Guid projectId, Guid taskId);
    }

    public class ProjectTaskRepository : Repository<ProjectTask>, IProjectTaskRepository
    {
        public ProjectTaskRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public async Task<List<ProjectTask>> GetAllFromProjectAsync(Guid projectId)
        {
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

        public Task<ProjectTask> GetProjectTaskAsync(Guid projectId, Guid taskId)
        {
            return  EntitySet
                .AsNoTracking()
                .Where(pt => pt.ProjectId == projectId && pt.TaskId == taskId)
                .FirstOrDefaultAsync();
        }
    }
}