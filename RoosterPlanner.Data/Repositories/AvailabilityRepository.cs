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
        Task<List<Availability>> GetScheduledAvailabilities(Guid projectId, DateTime dateTime);
        Task<Availability> GetAvailability(Guid participationId, Guid shiftId);
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
                .Include(a => a.Participation)
                .Include(a => a.Shift)
                .ThenInclude(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Where(a => a.Participation.ProjectId == projectId && a.Participation.PersonId == userId)
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
                .Include(a => a.Shift)
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
                .Include(a => a.Participation)
                .ThenInclude(p=>p.Project)
                .Include(a => a.Shift)
                .ThenInclude(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Where(a => a.ParticipationId == participationId && a.Type == AvailibilityType.Scheduled)
                .OrderBy(a => a.Shift.Date)
                .ToListAsync();
            availabilities.ForEach(a =>
            {
                a.Shift.Availabilities = null;
                a.Shift.Task.Shifts = null;
                a.Participation.Availabilities = null;
            });
            return availabilities;
        }

        public async Task<List<Availability>> GetScheduledAvailabilities(Guid projectId, DateTime dateTime)
        {
            if (projectId == Guid.Empty)
                return null;
            List<Availability> availabilities = await EntitySet
                .AsNoTracking()
                .Include(a => a.Participation)
                .ThenInclude(p => p.Person)
                .Include(a => a.Shift)
                .ThenInclude(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Where(a => a.Participation.ProjectId == projectId && a.Type == AvailibilityType.Scheduled &&
                            a.Shift.Date == dateTime)
                .OrderBy(a => a.Shift.StartTime)
                .ThenBy(a => a.Shift.EndTime)
                .ToListAsync();
            availabilities.ForEach(a =>
            {
                a.Shift.Availabilities = null;
                a.Shift.Task.Shifts = null;
                a.Participation.Person.Participations = null;
                a.Participation.Availabilities = null;
            });
            return availabilities;
        }

        public Task<Availability> GetAvailability(Guid participationId, Guid shiftId)
        {
            return EntitySet
                .AsNoTracking()
                .Where(a => a.ParticipationId == participationId && a.ShiftId == shiftId)
                .FirstOrDefaultAsync();
        }
    }
}