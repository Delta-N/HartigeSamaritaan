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
        Task<List<Participation>> GetActiveParticiationsAsync(Guid personGuid);
    }

    public class ParticipationRepository : Repository<Participation>, IParticipationRepository
    {
        //Constructor
        public ParticipationRepository(RoosterPlannerContext dataContext, ILogger logger) : base(dataContext, logger)
        {
        }

        public Task<List<Participation>> GetActiveParticiationsAsync(Guid personGuid)
        {
            return this.EntitySet
                .Include(x => x.Availabilities)
                //.Include(x=>x.IsWantedBy)
                .Include(x => x.WantsToWorkWith)
                .Where(p => p.PersonId == personGuid && !p.Project.Closed)
                .ToListAsync();
        }
    }
}
