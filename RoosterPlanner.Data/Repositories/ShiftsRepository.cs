using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public interface IShiftRepository : IRepository<Shift>
    {
        Task<List<Shift>> GetByProjectAsync(Guid projectId);
        Task<List<Shift>> AddShiftsAsync(List<Shift> shifts);
        Task<Shift> GetShiftAsync(Guid shiftId);
        Task<List<Shift>> GetByProjectUserAndDateAsync(Guid projectId, Guid userId, DateTime date);
        Task<List<Shift>> GetByProjectWithAvailabilitiesAsync(Guid projectId, Guid userId);
    }

    public class ShiftRepository : Repository<Shift>, IShiftRepository
    {
        //Constructor
        public ShiftRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Task<List<Shift>> GetByProjectAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return Task.FromResult<List<Shift>>(null);
            return EntitySet
                .AsNoTracking()
                .Include(s => s.Project)
                .Include(s => s.Task)
                .Where(s => s.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<Shift>> GetByProjectWithAvailabilitiesAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty)
                return await Task.FromResult<List<Shift>>(null);
            List<Shift> shifts = await EntitySet
                .AsNoTracking()
                .Include(s => s.Project)
                .Include(s => s.Task)
                .Include(s => s.Availabilities)
                .ThenInclude(a => a.Participation)
                .Where(s => s.ProjectId == projectId)
                .OrderBy(s => s.Date)
                .ToListAsync();

            foreach (Shift shift in shifts)
            {
                List<Availability> toBeRemoved = new List<Availability>();
                foreach (Availability shiftAvailability in shift.Availabilities)
                {
                    if (shiftAvailability.Participation != null)
                    {
                        if (shiftAvailability.Participation.PersonId != userId)
                            toBeRemoved.Add(shiftAvailability);
                        shiftAvailability.Participation.Availabilities = null;
                    }

                    shiftAvailability.Shift = null;
                }
                toBeRemoved.ForEach(a=>shift.Availabilities.Remove(a));

                if (shift.Task != null)
                    shift.Task.Shifts = null;
            }

            return shifts;
        }

        public Task<List<Shift>> AddShiftsAsync(List<Shift> shifts)
        {
            if (shifts == null || shifts.Count == 0)
                return Task.FromResult<List<Shift>>(null);
            foreach (Shift shift in shifts)
            {
                if (shift.Id == Guid.Empty)
                    shift.SetKey(Guid.NewGuid());
                shift.LastEditDate = DateTime.UtcNow;
                EntitySet.AddAsync(shift);
            }

            return Task.FromResult(shifts);
        }

        public Task<Shift> GetShiftAsync(Guid shiftId)
        {
            if (shiftId == Guid.Empty)
                return Task.FromResult<Shift>(null);

            return EntitySet
                .AsNoTracking()
                .Include(s => s.Task)
                .Include(s => s.Project)
                .Where(s => s.Id == shiftId).FirstOrDefaultAsync();
        }

        public async Task<List<Shift>> GetByProjectUserAndDateAsync(Guid projectId, Guid userId, DateTime date)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                return null;
            List<Shift> listOfShifts = await EntitySet
                .AsNoTracking()
                .Include(s => s.Project)
                .Include(s => s.Task)
                .Include(s => s.Availabilities)
                .ThenInclude(a => a.Participation)
                .Where(s => s.ProjectId == projectId && s.Date == date)
                .ToListAsync();
            Parallel.ForEach(listOfShifts, (shift) =>
            {
                shift.Availabilities = shift.Availabilities.Where(a => a.Participation.PersonId == userId).ToList();
                shift.Task.Shifts = null;
                shift.Availabilities.ForEach(a =>
                {
                    a.Participation.Availabilities = null;
                    a.Shift = null;
                });
            });
            return listOfShifts; 
        }
    }
}