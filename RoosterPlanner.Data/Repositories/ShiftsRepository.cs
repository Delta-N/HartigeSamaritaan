using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IShiftRepository : IRepository<Models.Shift>
    {
        Task<List<Models.Shift>> GetActiveShiftsForProjectAsync(Guid projectId);
    }

    public class ShiftRepository : Repository<Models.Shift>, IShiftRepository
    {
        //Constructor
        public ShiftRepository(RoosterPlannerContext dataContext, ILogger logger) : base(dataContext, logger)
        {
        }

        public Task<List<Models.Shift>> GetActiveShiftsForProjectAsync(Guid projectId)
        {
            var bla = this.EntitySet.Where(i => i.Date > DateTime.Now
            && i.Task.DeletedDateTime == null
            && i.Task.TaskProjects.Any(x => x.ProjectId == projectId))
            .OrderBy(i => i.Date).ThenBy(i => i.StartTime)
                .Include(t => t.Task)
                .Include("Task.Category");
            
            return bla.ToListAsync();
        }
    }
}
