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
    public interface IProjectRepository : IRepository<Project>
    {
        /// <summary>
        /// Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        Task<List<Project>> GetActiveProjectsAsync();
    }

    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        //Constructor
        public ProjectRepository(RoosterPlannerContext dataContext, ILogger logger) : base(dataContext, logger)
        {
        }

        /// <summary>
        /// Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public Task<List<Project>> GetActiveProjectsAsync()
        {
            return this.EntitySet.Where(p => !p.Closed).ToListAsync();
        }
    }
}
