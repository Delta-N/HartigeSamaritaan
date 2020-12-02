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
        Task<List<Shift>> AddAll(List<Shift> shifts);
        Task<Shift> GetShift(Guid shiftId);
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

        public async Task<List<Shift>> AddAll(List<Shift> shifts)
        {
            if (shifts == null || shifts.Count == 0)
                return await Task.FromResult<List<Shift>>(null);
            foreach (Shift shift in shifts)
            {
                if (shift.Id == Guid.Empty)
                    shift.SetKey(Guid.NewGuid());
                await EntitySet.AddAsync(shift);
            }

            return shifts;
        }

        public Task<Shift> GetShift(Guid shiftId)
        {
            if (shiftId == Guid.Empty)
                return Task.FromResult<Shift>(null);

            return EntitySet
                .AsNoTracking()
                .Include(s => s.Task)
                .Include(s => s.Project)
                .Where(s=>s.Id==shiftId).FirstOrDefaultAsync();
        }
    }
}