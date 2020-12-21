using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IAvailabilityRepository : IRepository<Availability>
    {
        Task<List<Availability>> FindAvailabilitiesAsync(Guid projectId, Guid userId);
    }

    public class AvailabilityRepository : Repository<Availability>, IAvailabilityRepository
    {
        //Constructor
        public AvailabilityRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Task<List<Availability>> FindAvailabilitiesAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                return null;
            return EntitySet
                .AsNoTracking()
                .Include(a=>a.Participation)
                .Include(a=>a.Shift)
                .Where(a => a.Participation.ProjectId == projectId && a.Participation.PersonId==userId)
                .ToListAsync();
        }
    }
}