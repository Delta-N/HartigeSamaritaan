using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Types;

namespace RoosterPlanner.Data.Repositories
{
    public interface IAvailabilityRepository : IRepository<Availability>
    {
        Task<List<Availability>> FindAvailabilitiesAsync(Guid projectId, Guid userId);
        Task<List<Availability>> GetActiveAvailabilities(Guid participationId);
        Task<List<Availability>> GetScheduledAvailabilities(Guid participationId);
    }

    public class AvailabilityRepository : Repository<Availability>, IAvailabilityRepository
    {
        //Constructor
        public AvailabilityRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public async Task<List<Availability>> FindAvailabilitiesAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                return null;
            List<Availability> availabilities = await EntitySet
                .AsNoTracking()
                .Include(a=>a.Participation)
                .Include(a=>a.Shift)
                .ThenInclude(s=>s.Task)
                .Where(a => a.Participation.ProjectId == projectId && a.Participation.PersonId==userId)
                .ToListAsync();
            availabilities.ForEach(a =>
            {
                a.Participation.Availabilities = null;
                a.Shift.Availabilities = null;
                a.Shift.Task.Shifts = null;
            });

            return availabilities;
        }

        public async Task<List<Availability>> GetActiveAvailabilities(Guid participationId)
        {
            if (participationId == Guid.Empty)
                return null;
            List<Availability> availabilities = await EntitySet
                .AsNoTracking()
                .Include(a=>a.Shift)
                .Where(a => a.ParticipationId == participationId && a.Type == AvailibilityType.Scheduled &&
                            a.Shift.Date >= DateTime.Today)
                .ToListAsync();
            availabilities.ForEach(a =>
            {
                a.Shift.Availabilities = null;
                a.Shift.Task.Shifts = null;
            });
            return availabilities;
        }

        public async Task<List<Availability>> GetScheduledAvailabilities(Guid participationId)
        {
            if (participationId == Guid.Empty)
                return null;
            List<Availability> availabilities = await EntitySet
                .AsNoTracking()
                .Include(a => a.Shift)
                .ThenInclude(s=>s.Task)
                .Where(a => a.ParticipationId == participationId && a.Type == AvailibilityType.Scheduled)
                .OrderBy(a=>a.Shift.Date)
                .ToListAsync();
            availabilities.ForEach(a =>
            {
                a.Shift.Availabilities = null;
                a.Shift.Task.Shifts = null;
            });
            return availabilities;
        }
    }
}