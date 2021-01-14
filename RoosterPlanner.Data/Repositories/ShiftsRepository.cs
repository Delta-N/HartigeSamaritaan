using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Models.Models;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public interface IShiftRepository : IRepository<Shift>
    {
        Task<List<Shift>> GetByProjectAsync(Guid projectId);
        Task<List<Shift>> AddShiftsAsync(List<Shift> shifts);
        Task<Shift> GetShiftAsync(Guid shiftId);
        Task<Shift> GetShiftWithAvailabilitiesAsync(Guid shiftId);
        Task<List<Shift>> GetByProjectAndDateWithAvailabilitiesAsync(Guid projectId, DateTime date);
        Task<List<Shift>> GetByProjectUserAndDateAsync(Guid projectId, Guid userId, DateTime date);
        Task<List<Shift>> GetByProjectWithAvailabilitiesAsync(Guid projectId);
        Task<List<Shift>> GetByProjectWithAvailabilitiesAsync(Guid projectId, Guid userId);
        Task<List<Shift>> SearchProjectsAsync(ShiftFilter filter);
        Task<ShiftData> GetUniqueDataAsync(Guid projectId);
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
                .ThenInclude(t=>t.Instruction)
                .Where(s => s.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<Shift>> GetByProjectWithAvailabilitiesAsync(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return await Task.FromResult<List<Shift>>(null);
            List<Shift> shifts = await EntitySet
                .AsNoTracking()
                .Include(s => s.Project)
                .Include(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Availabilities)
                .Where(s => s.ProjectId == projectId && s.Date>=DateTime.Today)
                .OrderBy(s => s.Date)
                .ToListAsync();

            foreach (Shift shift in shifts)
            {
                foreach (Availability shiftAvailability in shift.Availabilities)
                {
                    shiftAvailability.Shift = null;
                }

                if (shift.Task != null)
                    shift.Task.Shifts = null;
            }

            return shifts;
        }

        public async Task<List<Shift>> GetByProjectWithAvailabilitiesAsync(Guid projectId, Guid userId)
        {
            if (projectId == Guid.Empty)
                return await Task.FromResult<List<Shift>>(null);
            List<Shift> shifts = await EntitySet
                .AsNoTracking()
                .Include(s => s.Project)
                .Include(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Availabilities)
                .ThenInclude(a => a.Participation)
                .Where(s => s.ProjectId == projectId && s.Date>=DateTime.Today)
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

                toBeRemoved.ForEach(a => shift.Availabilities.Remove(a));

                if (shift.Task != null)
                    shift.Task.Shifts = null;
            }

            return shifts;
        }

        public async Task<ShiftData> GetUniqueDataAsync(Guid projectId)
        {
            if(projectId==Guid.Empty)
                throw new ArgumentNullException(nameof(projectId));
            ShiftData data = new ShiftData();

            var q = EntitySet
                .AsNoTracking()
                .Include(s => s.Task)
                .Where(s=>s.ProjectId==projectId);
            data.Tasks =  await q.Select(s => s.Task).Distinct().ToListAsync();
            data.Dates = await q.Select(s => s.Date).Distinct().ToListAsync();
            data.StartTimes = await q.Select(s => s.StartTime.ToString("hh\\:mm")).Distinct().ToListAsync();
            data.EndTimes = await q.Select(s => s.EndTime.ToString("hh\\:mm")).Distinct().ToListAsync();
            data.ParticipantsRequired = await q.Select(s => s.ParticipantsRequired).Distinct().ToListAsync();
            return data;
        }

        public Task<List<Shift>> SearchProjectsAsync(ShiftFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            var q = EntitySet
                .AsNoTracking()
                .Include(s=>s.Task)
                .Where(s=>s.ProjectId==filter.ProjectId);
            q = filter.SetFilter(q);
           

            q = q.Where(x => filter.Tasks.Any(t=>t==x.TaskId.ToString()));
            q = q.Where(x => x.Date >= filter.Date);
            
            q = q.Where(x => x.StartTime >= TimeSpan.Parse(filter.Start));
            q = q.Where(x => x.EndTime >= TimeSpan.Parse(filter.End));
            q = q.Where(x => x.ParticipantsRequired >= filter.ParticipantsRequired);
            q = filter.SetFilter(q);
            filter.TotalItemCount = q.Count();
            Task<List<Shift>> shifts;
            if (filter.Offset >= 0 && filter.PageSize != 0)
                shifts = q.Skip(filter.Offset).Take(filter.PageSize).ToListAsync();
            else
                shifts = q.ToListAsync();

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
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Project)
                .Where(s => s.Id == shiftId).FirstOrDefaultAsync();
        }

        public async Task<Shift> GetShiftWithAvailabilitiesAsync(Guid shiftId)
        {
            if (shiftId == Guid.Empty)
                return await Task.FromResult<Shift>(null);

            Shift shift = await EntitySet
                .AsNoTracking()
                .Include(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Project)
                .Include(s => s.Availabilities)
                .ThenInclude(a => a.Participation)
                .Where(s => s.Id == shiftId)
                .FirstOrDefaultAsync();

            shift.Task.Shifts = null;
            shift.Availabilities.ForEach(a =>
            {
                if (a.Participation != null)
                    a.Participation.Availabilities = null;
                a.Shift = null;
            });

            return shift;
        }

        public async Task<List<Shift>> GetByProjectAndDateWithAvailabilitiesAsync(Guid projectId, DateTime date)
        {
            if (projectId == Guid.Empty)
                return null;
            List<Shift> listOfShifts = await EntitySet
                .AsNoTracking()
                .Include(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Availabilities)
                .Where(s => s.ProjectId == projectId && s.Date == date && s.Task != null)
                .ToListAsync();
            listOfShifts?.ForEach(shift =>
            {
                if (shift.Task != null)
                    shift.Task.Shifts = null;
                shift.Availabilities.ForEach(a => { a.Shift = null; });
            });
            return listOfShifts;
        }

        public async Task<List<Shift>> GetByProjectUserAndDateAsync(Guid projectId, Guid userId, DateTime date)
        {
            if (projectId == Guid.Empty || userId == Guid.Empty)
                return null;
            List<Shift> listOfShifts = await EntitySet
                .AsNoTracking()
                .Include(s => s.Project)
                .Include(s => s.Task)
                .ThenInclude(t => t.Instruction)
                .Include(s => s.Availabilities)
                .ThenInclude(a => a.Participation)
                .Where(s => s.ProjectId == projectId && s.Date == date && s.Task!=null)
                .ToListAsync();

            Parallel.ForEach(listOfShifts, shift =>
            {
                shift.Availabilities = shift.Availabilities.Where(a => a.Participation.PersonId == userId).ToList();
                if (shift.Task != null)
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