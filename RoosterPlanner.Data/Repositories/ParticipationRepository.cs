using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IParticipationRepository : IRepository<Participation>
    {
        Task<List<Participation>> GetActiveParticipationsAsync(Guid personId);
        Task<Participation> GetSpecificParticipationAsync(Guid personId, Guid projectId);
        Task<List<Participation>> GetParticipations(Guid projectId);
        Task<List<Participation>> GetParticipationsWithAvailabilities(Guid projectId);
    }

    public class ParticipationRepository : Repository<Participation>, IParticipationRepository
    {
        //Constructor
        public ParticipationRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Task<List<Participation>> GetActiveParticipationsAsync(Guid personId)
        {
            return EntitySet
                .AsNoTracking()
                .Include(p => p.Project)
                .Include(p => p.Person)
                .Where(p => p.PersonId == personId && !p.Project.Closed && p.Active && (
                    p.Project.ParticipationEndDate >= DateTime.Now ||
                    p.Project.ParticipationEndDate == null))
                .ToListAsync();
        }

        public Task<Participation> GetSpecificParticipationAsync(Guid personId, Guid projectId)
        {
            return EntitySet
                .AsNoTracking()
                .Include(p => p.Project)
                .ThenInclude(p=>p.PictureUri)
                .Include(p => p.Person)
                .Where(p => p.PersonId == personId && p.ProjectId == projectId)
                .FirstOrDefaultAsync();
        }

        public Task<List<Participation>> GetParticipations(Guid projectId)
        {
            return EntitySet
                .AsNoTracking()
                .Include(p=>p.Project)
                .Where(p => p.ProjectId == projectId)
                .ToListAsync();
        }

        public Task<List<Participation>> GetParticipationsWithAvailabilities(Guid projectId)
        {
            return EntitySet
                .Include(p=>p.Project)
                .Include(p=>p.Availabilities)
                .ThenInclude(a=>a.Shift)
                .ThenInclude(s=>s.Task)
                .Where(p => p.ProjectId == projectId)
                .ToListAsync();
        }
    }
}