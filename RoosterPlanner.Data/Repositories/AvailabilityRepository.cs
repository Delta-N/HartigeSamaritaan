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
        /// <summary>
        /// Get a list of availabilties based on a projectId and a personId including related attributes.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="personId"></param>
        /// <returns>A task of a list of availabilites</returns>
        Task<List<Availability>> FindAvailabilitiesAsync(Guid projectId, Guid personId);

        /// <summary>
        /// Get a list of availabilites where the type is 'scheduled' and the date is not in the past based on a participationId.
        /// </summary>
        /// <param name="participationId"></param>
        /// <returns>A task of a list of availabilites</returns>
        Task<List<Availability>> GetActiveAvailabilities(Guid participationId);

        /// <summary>
        /// Get a list of availabilites where the type is 'scheduled' based on a participationId.
        /// </summary>
        /// <param name="participationId"></param>
        /// <returns>A task of a list of availabilites</returns>
        Task<List<Availability>> GetScheduledAvailabilities(Guid participationId);

        /// <summary>
        /// Get a list of availabilites based on a projectId and a dateTime that are scheduled.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="dateTime"></param>
        /// <returns>A task of a list of availabilites</returns>
        Task<List<Availability>> GetScheduledAvailabilities(Guid projectId, DateTime dateTime);

        /// <summary>
        /// Get a specific availability based on an participationId and shiftId
        /// </summary>
        /// <param name="participationId"></param>
        /// <param name="shiftId"></param>
        /// <returns>A task of an availability</returns>
        Task<Availability> GetAvailability(Guid participationId, Guid shiftId);
    }

    public class AvailabilityRepository : Repository<Availability>, IAvailabilityRepository
    {
        //Constructor
        public AvailabilityRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Get a list of availabilties based on a projectId and a personId including related attributes.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="personId"></param>
        /// <returns>A task of a list of availabilites</returns>
        public async Task<List<Availability>> FindAvailabilitiesAsync(Guid projectId, Guid personId)
        {
            if (projectId == Guid.Empty || personId == Guid.Empty)
                return null;
            List<Availability> availabilities = await EntitySet
                .AsNoTracking()
                .Include(a => a.Participation)
                .Include(a => a.Shift)
                .ThenInclude(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Where(a => a.Participation.ProjectId == projectId &&
                            a.Participation.PersonId == personId)
                .ToListAsync();
            availabilities.ForEach(a =>
            {
                if (a.Participation != null)
                    a.Participation.Availabilities = null;
                if (a.Shift != null)
                {
                    a.Shift.Availabilities = null;
                    if (a.Shift.Task != null)
                        a.Shift.Task.Shifts = null;
                }
            });

            return availabilities;
        }

        /// <summary>
        /// Get a list of availabilites where the type is 'scheduled' and the date is not in the past based on a participationId.
        /// </summary>
        /// <param name="participationId"></param>
        /// <returns>A task of a list of availabilites</returns>
        public async Task<List<Availability>> GetActiveAvailabilities(Guid participationId)
        {
            if (participationId == Guid.Empty)
                return null;
            List<Availability> availabilities = await EntitySet
                .AsNoTracking()
                .Include(a => a.Shift)
                .Where(a => a.ParticipationId == participationId &&
                            a.Type == AvailibilityType.Scheduled &&
                            a.Shift.Date >= DateTime.Today)
                .ToListAsync();
            availabilities.ForEach(a => { a.Shift.Availabilities = null; });
            return availabilities;
        }

        /// <summary>
        /// Get a list of availabilites where the type is 'scheduled' based on a participationId.
        /// </summary>
        /// <param name="participationId"></param>
        /// <returns>A task of a list of availabilites</returns>
        public async Task<List<Availability>> GetScheduledAvailabilities(Guid participationId)
        {
            if (participationId == Guid.Empty)
                return null;
            List<Availability> availabilities = await EntitySet
                .AsNoTracking()
                .Include(a => a.Participation)
                .ThenInclude(p => p.Project)
                .Include(a => a.Shift)
                .ThenInclude(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Where(a => a.ParticipationId == participationId &&
                            a.Type == AvailibilityType.Scheduled)
                .OrderBy(a => a.Shift.Date)
                .ToListAsync();
            availabilities.ForEach(a =>
            {
                if (a.Shift == null) return;
                if (a.Shift.Task != null)
                    a.Shift.Task.Shifts = null;

                a.Shift.Availabilities = null;
                if (a.Participation != null)
                    a.Participation.Availabilities = null;
            });
            return availabilities;
        }

        /// <summary>
        /// Get a list of availabilites based on a projectId and a dateTime that are scheduled.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="dateTime"></param>
        /// <returns>A task of a list of availabilites</returns>
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
                .Where(a => a.Participation.ProjectId == projectId &&
                            a.Type == AvailibilityType.Scheduled &&
                            a.Shift.Date == dateTime)
                .OrderBy(a => a.Shift.StartTime)
                .ThenBy(a => a.Shift.EndTime)
                .ToListAsync();
            availabilities.ForEach(a =>
            {
                a.Shift.Availabilities = null;
                if (a.Shift.Task != null)
                    a.Shift.Task.Shifts = null;
                a.Participation.Person.Participations = null;
                a.Participation.Availabilities = null;
            });
            return availabilities;
        }

        /// <summary>
        /// Get a specific availability based on an participationId and shiftId
        /// </summary>
        /// <param name="participationId"></param>
        /// <param name="shiftId"></param>
        /// <returns>A task of an availability</returns>
        public Task<Availability> GetAvailability(Guid participationId, Guid shiftId)
        {
            if (participationId == Guid.Empty || shiftId == Guid.Empty)
                return null;
            return EntitySet
                .AsNoTracking()
                .Where(a => a.ParticipationId == participationId &&
                            a.ShiftId == shiftId)
                .FirstOrDefaultAsync();
        }
    }
}