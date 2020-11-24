using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IShiftRepository : IRepository<Shift>
    {
        Task<List<Shift>> GetAll(Guid projectId);
        Task<List<Shift>> AddAll(List<Shift> shifts);
    }

    public class ShiftRepository : Repository<Shift>, IShiftRepository
    {
        //Constructor
        public ShiftRepository(RoosterPlannerContext dataContext) : base(dataContext)
        {
        }

        public Task<List<Shift>> GetAll(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return null;
            return this.EntitySet
                .AsNoTracking()
                .AsQueryable()
                .Include(s => s.Project)
                .Include(s => s.Task)
                .Where(s => s.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<Shift>> AddAll(List<Shift> shifts)
        {
            if (shifts == null || shifts.Count == 0)
                return null;
            await this.EntitySet.AddRangeAsync(shifts);
            return shifts;
        }
    }
}