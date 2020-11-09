using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Models;
using RoosterPlanner.Models.FilterModels;

namespace RoosterPlanner.Data.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        /// <summary>
        /// Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        Task<List<Project>> GetActiveProjectsAsync();

        /// <summary>
        /// Search for projects based on given filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<List<Project>> SearchProjectsAsync(ProjectFilter filter);

        Task<Project> GetProjectDetails(Guid id);
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
            return this.EntitySet.Where(p => !p.Closed).OrderBy(p => p.StartDate).ToListAsync();
        }

        /// <summary>
        /// Search for projects based on given filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<List<Project>> SearchProjectsAsync(ProjectFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            var q = this.EntitySet.AsNoTracking().AsQueryable();

            //Name
            if (!String.IsNullOrEmpty(filter.Name))
                q = q.Where(x => x.Name.IndexOf(filter.Name) >= 0);

            //City
            if (!String.IsNullOrEmpty(filter.City))
                q = q.Where(x => x.City.IndexOf(filter.City) >= 0);

            //StartDate
            if (filter.StartDate.HasValue)
                q = q.Where(x => x.StartDate >= filter.StartDate.Value);

            //EndDate
            if (filter.EndDate.HasValue)
                q = q.Where(x => x.EndDate >= filter.EndDate.Value || x.EndDate == null);

            //Closed
            if (filter.Closed.HasValue)
                q = q.Where(x => x.Closed == filter.Closed.Value);

            q = filter.SetFilter<Project>(q);

            filter.TotalItemCount = q.Count();

            Task<List<Project>> projects = null;
            if (filter.Offset >= 0 && filter.PageSize != 0)
                projects = q.Skip(filter.Offset).Take(filter.PageSize).ToListAsync();
            else
                projects = q.ToListAsync();

            return projects;
        }

        public Task<Project> GetProjectDetails(Guid id)
        {
            return this.EntitySet.Include(x => x.ProjectTasks)
                .Where(p => p.Id == id).FirstOrDefaultAsync();
        }
    }
}