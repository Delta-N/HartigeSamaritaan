﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        /// <summary>
        /// Search for projects based on given filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A task of a list of projects.</returns>
        Task<List<Project>> SearchProjectsAsync(ProjectFilter filter);

        /// <summary>
        /// Get ProjectDetails based on an id including projectTasks and a projectPicture.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A task of a project.</returns>
        Task<Project> GetProjectDetailsAsync(Guid id);
    }

    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        //Constructor
        public ProjectRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Search for projects based on given filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A task of a list of projects.</returns>
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

        /// <summary>
        /// Get ProjectDetails based on an id including projectTasks and a projectPicture.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A task of a project.</returns>
        public Task<Project> GetProjectDetailsAsync(Guid id)
        {
            return EntitySet
                .Include(x => x.ProjectTasks)
                .Include(p=>p.PictureUri)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}