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
                .Include(p => p.WantsToWorkWith)
                .Where(p => p.PersonId == personId && !p.Project.Closed && (
                    p.Project.ParticipationEndDate >= DateTime.Now ||
                    p.Project.ParticipationEndDate == null))
                .ToListAsync();
        }

        public Task<Participation> GetSpecificParticipationAsync(Guid personId, Guid projectId)
        {
            return EntitySet
                .AsNoTracking()
                .Include(p => p.Project)
                .Include(p => p.Person)
                .Include(p => p.WantsToWorkWith)
                .Where(p => p.PersonId == personId && p.ProjectId == projectId)
                .FirstOrDefaultAsync();
        }
    }
}