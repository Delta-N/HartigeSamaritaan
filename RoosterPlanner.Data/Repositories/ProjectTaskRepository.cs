﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IProjectTaskRepository : IRepository<ProjectTask>
    {
        Task<List<ProjectTask>> GetAll(Guid projectId);
        Task<ProjectTask> GetProjectTask(Guid projectId, Guid taskId);
    }

    public class ProjectTaskRepository : Repository<ProjectTask>, IProjectTaskRepository
    {
        public ProjectTaskRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Task<List<ProjectTask>> GetAll(Guid projectId)
        {
            return this.EntitySet
                .AsNoTracking()
                .AsQueryable()
                .Include(pt => pt.Project)
                .Include(pt => pt.Task)
                .Where(pt => pt.ProjectId == projectId).ToListAsync();
        }

        public Task<ProjectTask> GetProjectTask(Guid projectId, Guid taskId)
        {
            return  this.EntitySet
                .AsNoTracking()
                .AsQueryable()
                .Where(pt => pt.ProjectId == projectId && pt.TaskId == taskId)
                .FirstOrDefaultAsync();
        }
    }
}