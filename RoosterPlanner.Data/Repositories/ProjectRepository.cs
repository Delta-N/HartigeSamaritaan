using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
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

        Task<Project> GetProjectDetailsAsync(Guid id);
    }

    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        //Constructor
        public ProjectRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public Task<List<Project>> GetActiveProjectsAsync()
        {
            return EntitySet.Where(p => !p.Closed).OrderBy(p => p.ParticipationStartDate).ToListAsync();
        }

        /// <summary>
        /// Search for projects based on given filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<List<Project>> SearchProjectsAsync(ProjectFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var q = EntitySet
                .AsNoTracking();

            //Name
            if (!string.IsNullOrEmpty(filter.Name))
                q = q.Where(x => x.Name.Contains(filter.Name));

            //City
            if (!string.IsNullOrEmpty(filter.City))
                q = q.Where(x => x.City.Contains(filter.City));

            //StartDate
            if (filter.StartDate.HasValue)
                q = q.Where(x => x.ParticipationStartDate >= filter.StartDate.Value);

            //EndDate
            if (filter.EndDate.HasValue)
                q = q.Where(x => x.ParticipationEndDate >= filter.EndDate.Value || x.ParticipationEndDate == null);

            //Closed
            if (filter.Closed.HasValue)
                q = q.Where(x => x.Closed == filter.Closed.Value);

            q = filter.SetFilter(q);

            filter.TotalItemCount = q.Count();

            Task<List<Project>> projects;
            if (filter.Offset >= 0 && filter.PageSize != 0)
                projects = q.Skip(filter.Offset).Take(filter.PageSize).ToListAsync();
            else
                projects = q.ToListAsync();

            return projects;
        }

        public Task<Project> GetProjectDetailsAsync(Guid id)
        {
            return EntitySet.Include(x => x.ProjectTasks)
                .Where(p => p.Id == id).FirstOrDefaultAsync();
        }
    }
}