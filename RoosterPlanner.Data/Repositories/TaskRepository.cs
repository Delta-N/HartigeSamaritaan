using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Models.FilterModels;
using Task = RoosterPlanner.Models.Task;

namespace RoosterPlanner.Data.Repositories
{
    public interface ITaskRepository : IRepository<Task>
    {
        Task<List<Task>> SearchTasksAsync(TaskFilter filter);
        Task<Task> GetTask(Guid id);
    }

    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        //Constructor
        public TaskRepository(RoosterPlannerContext dataContext) : base(dataContext)
        {
        }

        public Task<List<Task>> SearchTasksAsync(TaskFilter filter)
        {
            if(filter==null)
                throw new ArgumentNullException("filter");

            IQueryable<Task> queryable = EntitySet.AsNoTracking().AsQueryable().Include(t=>t.Category);
            
            //Name
            if (!String.IsNullOrEmpty(filter.Name))
                queryable = queryable.Where(t => t.Name.IndexOf(filter.Name) >= 0);

            filter.TotalItemCount = queryable.Count();
            Task<List<Task>> tasks = null;
            if (filter.Offset > 0 && filter.PageSize != 0)
                tasks = queryable.Skip(filter.Offset).Take(filter.PageSize).ToListAsync();
            else
            {
                tasks = queryable.ToListAsync();
            }

            return tasks;
        }

        public Task<Task> GetTask(Guid id)
        {
            return EntitySet
                .Include(t => t.Category)
                .Include(t => t.Requirements)
                .Where(t => t.Id == id).FirstOrDefaultAsync();
        }
    }
}
