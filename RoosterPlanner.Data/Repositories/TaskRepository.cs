using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
using Task = RoosterPlanner.Models.Task;

namespace RoosterPlanner.Data.Repositories
{
    public interface ITaskRepository : IRepository<Task>
    {
        Task<List<Task>> GetActiveTasksAsync();
    }

    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        //Constructor
        public TaskRepository(RoosterPlannerContext dataContext) : base(dataContext)
        {
        }

        public Task<List<Task>> GetActiveTasksAsync()
        {
            return EntitySet.Where(t => !t.DeletedDateTime.HasValue || t.DeletedDateTime >= DateTime.UtcNow)
                .Include(t => t.Category)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
