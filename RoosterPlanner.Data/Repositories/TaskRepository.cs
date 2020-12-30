using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;

namespace RoosterPlanner.Data.Repositories
{
    public interface ITaskRepository : IRepository<Task>
    {
        System.Threading.Tasks.Task<List<Task>> SearchTasksAsync(TaskFilter filter);
        System.Threading.Tasks.Task<Task> GetTaskAsync(Guid id);
    }

    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        //Constructor
        public TaskRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public System.Threading.Tasks.Task<List<Task>> SearchTasksAsync(TaskFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            IQueryable<Task> queryable = EntitySet
                .AsNoTracking()
                .Include(t => t.Category)
                .Include(t => t.Instruction);

            //Name
            if (!string.IsNullOrEmpty(filter.Name))
                queryable = queryable.Where(t => t.Name.Contains(filter.Name));

            queryable = filter.SetFilter(queryable);

            filter.TotalItemCount = queryable.Count();
            System.Threading.Tasks.Task<List<Task>> tasks;
            if (filter.Offset >= 0 && filter.PageSize != 0)
                tasks = queryable.Skip(filter.Offset).Take(filter.PageSize).ToListAsync();
            else
                tasks = queryable.ToListAsync();

            return tasks;
        }

        public System.Threading.Tasks.Task<Task> GetTaskAsync(Guid id)
        {
            return EntitySet
                .Include(t => t.Category)
                .Include(t => t.Instruction)
                .Where(t => t.Id == id).FirstOrDefaultAsync();
        }
    }
}