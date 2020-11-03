using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IParticipationRepository : IRepository<Participation>
    {
        Task<List<Participation>> GetActiveParticipationsAsync(Guid personGuid);
        Task<Participation> GetSpecificParticipation(Guid personGuid, Guid projectGuid);
    }

    public class ParticipationRepository : Repository<Participation>, IParticipationRepository
    {
        //Constructor
        public ParticipationRepository(RoosterPlannerContext dataContext, ILogger logger) : base(dataContext, logger)
        {
        }

        public Task<List<Participation>> GetActiveParticipationsAsync(Guid personGuid)
        {
            return this.EntitySet.AsNoTracking().AsQueryable()
                .Include(p => p.Project)
                .Include(p => p.Person)
                .Include(p => p.Availabilities)
                .Include(p => p.WantsToWorkWith)
                .Where(p => p.PersonId == personGuid && !p.Project.Closed)
                .ToListAsync();
        }

        public Task<Participation> GetSpecificParticipation(Guid personGuid, Guid projectGuid)
        {
            return this.EntitySet.AsNoTracking().AsQueryable()
                .Include(p => p.Project)
                .Include(p => p.Person)
                .Include(p => p.Availabilities)
                .Include(p => p.WantsToWorkWith)
                .Where(p => p.PersonId == personGuid && p.ProjectId == projectGuid)
                .FirstOrDefaultAsync();
        }
    }
}