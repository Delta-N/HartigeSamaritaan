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
    public interface IShiftRepository : IRepository<Shift>
    {
        Task<List<Shift>> GetActiveShiftsForProjectAsync(Guid projectId);
    }

    public class ShiftRepository : Repository<Shift>, IShiftRepository
    {
        //Constructor
        public ShiftRepository(RoosterPlannerContext dataContext, ILogger logger) : base(dataContext, logger)
        {
        }

        public Task<List<Shift>> GetActiveShiftsForProjectAsync(Guid projectId)
        {
            return EntitySet.Where(i => i.Date > DateTime.Now
            && i.Task.DeletedDateTime == null
            && i.Task.ProjectTasks.Any(x => x.ProjectId == projectId))
            .OrderBy(i => i.Date).ThenBy(i => i.StartTime)
                .Include(t => t.Task)
                .Include("Task.Category").ToListAsync();
        }
    }
}
