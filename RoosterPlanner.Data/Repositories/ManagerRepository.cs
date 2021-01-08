using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IManagerRepository : IRepository<Manager>
    {
        Task<Manager> GetManagerAsync(Guid projectId, Guid userId);
        Task<List<Manager>> UserManagesOtherProjectsAsync(Guid userId);
        Task<List<Manager>> GetProjectManagersAsync(Guid projectId);
        Task<List<Manager>> GetProjectsManagedByAsync(Guid userId);
    }

    public class ManagerRepository : Repository<Manager>, IManagerRepository
    {
        public ManagerRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Task<Manager> GetManagerAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                return null;

            return EntitySet
                .AsNoTracking()
                .Include(m => m.Person)
                .Include(m => m.Project)
                .Where(m => m.PersonId == userId && m.ProjectId == projectId)
                .FirstOrDefaultAsync();
        }

        public Task<List<Manager>> UserManagesOtherProjectsAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return null;
            return EntitySet
                .AsNoTracking()
                .Where(m => m.PersonId == userId)
                .ToListAsync();
        }

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

        public Task<List<Manager>> GetProjectsManagedByAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                return null;

            return EntitySet
                .AsNoTracking()
                .Include(m => m.Person)
                .Include(m => m.Project)
                .Where(m => m.PersonId == userId && m.Project.ProjectEndDate >= DateTime.Today && !m.Project.Closed)
                .ToListAsync();
        }
    }
}