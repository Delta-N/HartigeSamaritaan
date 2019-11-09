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
    public interface ITaskRepository : IRepository<Models.Task>
    {
        Task<List<Models.Task>> GetActiveTasksAsync();
    }

    public class TaskRepository : Repository<Models.Task>, ITaskRepository
    {
        //Constructor
        public TaskRepository(RoosterPlannerContext dataContext, ILogger logger) : base(dataContext, logger)
        {
        }

        public Task<List<Models.Task>> GetActiveTasksAsync()
        {
            return this.EntitySet.Where(t => !t.DeletedDateTime.HasValue || t.DeletedDateTime >= DateTime.UtcNow)
                .Include(t => t.Category)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
