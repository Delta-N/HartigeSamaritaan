using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IManagerRepository : IRepository<Manager>
    {
        /// <summary>
        /// Get a manager based on a projectId and a userId.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns>A task of a manager.</returns>
        Task<Manager> GetManagerAsync(Guid projectId, Guid userId);

        /// <summary>
        /// Get a list of managers (DTO for project and user) based on a userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A task of a list of managers.</returns>
        Task<List<Manager>> UserManagesOtherProjectsAsync(Guid userId);

        /// <summary>
        /// Get a list of manager (DTO for project and user) based on a projectId.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A task of a list of managers.</returns>
        Task<List<Manager>> GetProjectManagersAsync(Guid projectId);

        /// <summary>
        /// Get a list of 'active' managers based on a userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A task of a list of managers.</returns>
        Task<List<Manager>> GetProjectsManagedByAsync(Guid userId);
    }

    public class ManagerRepository : Repository<Manager>, IManagerRepository
    {
        public ManagerRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Get a manager based on a projectId and a userId.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns>A task of a manager.</returns>
        public Task<Manager> GetManagerAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                return null;

            return EntitySet
                .AsNoTracking()
                .Include(m => m.Person)
                .Include(m => m.Project)
                .Where(m => m.PersonId == userId &&
                            m.ProjectId == projectId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get a list of managers (DTO for project and user) based on a userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A task of a list of managers.</returns>
        public Task<List<Manager>> UserManagesOtherProjectsAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return null;
            return EntitySet
                .AsNoTracking()
                .Where(m => m.PersonId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Get a list of manager (DTO for project and user) based on a projectId.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A task of a list of managers.</returns>
        public Task<List<Manager>> GetProjectManagersAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return null;

            return EntitySet
                .AsNoTracking()
                .Include(m => m.Person)
                .Include(m => m.Project)
                .Where(m => m.ProjectId == projectId)
                .ToListAsync();
        }

        /// <summary>
        /// Get a list of 'active' managers based on a userId.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A task of a list of managers.</returns>
        public Task<List<Manager>> GetProjectsManagedByAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return null;

            return EntitySet
                .AsNoTracking()
                .Include(m => m.Person)
                .Include(m => m.Project)
                .Where(m => m.PersonId == userId &&
                            m.Project.ProjectEndDate >= DateTime.Today &&
                            !m.Project.Closed)
                .ToListAsync();
        }
    }
}