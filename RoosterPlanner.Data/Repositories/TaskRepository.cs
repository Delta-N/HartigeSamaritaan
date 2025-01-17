﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface ITaskRepository : IRepository<Task>
    {
        /// <summary>
        /// Search for tasks based on a filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A task of a list of tasks (datamodel)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        System.Threading.Tasks.Task<List<Task>> SearchTasksAsync(TaskFilter filter);

        /// <summary>
        /// Get a Task (datamodel) based on Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task of a Task(datamodel)</returns>
        System.Threading.Tasks.Task<Task> GetTaskAsync(Guid id);
    }

    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        //Constructor
        public TaskRepository(DbContext dataContext) : base(dataContext)
        {
        }
        /// <summary>
        /// Search for tasks based on a filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A task of a list of tasks (datamodel)</returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Get a Task (datamodel) based on Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task of a Task(datamodel)</returns>
        public async System.Threading.Tasks.Task<Task> GetTaskAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;
            Task task = await EntitySet
                .AsNoTracking()
                .Include(t => t.Category)
                .Include(t => t.Instruction)
                .Include(t => t.Requirements)
                .ThenInclude(r => r.CertificateType)
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (task.Category != null)
                task.Category.Tasks = null;
            foreach (Requirement taskRequirement in task.Requirements)
                taskRequirement.Task = null;

            return task;
        }
    }
}