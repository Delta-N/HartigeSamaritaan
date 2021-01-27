using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IParticipationRepository : IRepository<Participation>
    {
        /// <summary>
        /// Gets all active participations based on a personId.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>A task of a list of participations.</returns>
        Task<List<Participation>> GetActiveParticipationsAsync(Guid personId);

        /// <summary>
        /// Gets a specific participations based on a personId and a projectId.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="projectId"></param>
        /// <returns>A task of a participation.</returns>
        Task<Participation> GetSpecificParticipationAsync(Guid personId, Guid projectId);

        /// <summary>
        /// Gets a list of participations based on a projectId.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A task of a list of participations.</returns>
        Task<List<Participation>> GetParticipations(Guid projectId);

        /// <summary>
        /// Gets a list of participations based on a projectId including Availabilities.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A task of a list of participations.</returns>
        Task<List<Participation>> GetParticipationsWithAvailabilities(Guid projectId);
    }

    public class ParticipationRepository : Repository<Participation>, IParticipationRepository
    {
        //Constructor
        public ParticipationRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Gets al active participations based on a personId.
        /// </summary>
        /// <param name="personId"></param>
        /// <returns>A task of a list of participations.</returns>
        public Task<List<Participation>> GetActiveParticipationsAsync(Guid personId)
        {
            if (personId == Guid.Empty)
                return null;
            return EntitySet
                .AsNoTracking()
                .Include(p => p.Project)
                .Include(p => p.Person)
                .Where(p => p.PersonId == personId &&
                            !p.Project.Closed && p.Active &&
                            (p.Project.ParticipationEndDate >= DateTime.Now ||
                             p.Project.ParticipationEndDate == null)
                )
                .ToListAsync();
        }

        /// <summary>
        /// Gets a specific participations based on a personId and a projectId.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="projectId"></param>
        /// <returns>A task of a participation.</returns>
        public Task<Participation> GetSpecificParticipationAsync(Guid personId, Guid projectId)
        {
            if (personId == Guid.Empty || projectId == Guid.Empty)
                return null;
            return EntitySet
                .AsNoTracking()
                .Include(p => p.Project)
                .ThenInclude(p => p.PictureUri)
                .Include(p => p.Person)
                .Where(p => p.PersonId == personId &&
                            p.ProjectId == projectId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets a list of participations based on a projectId.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A task of a list of participations.</returns>
        public Task<List<Participation>> GetParticipations(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return null;
            return EntitySet
                .AsNoTracking()
                .Include(p => p.Project)
                .Where(p => p.ProjectId == projectId)
                .ToListAsync();
        }

        /// <summary>
        /// Gets a list of participations based on a projectId including Availabilities.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>A task of a list of participations.</returns>
        public Task<List<Participation>> GetParticipationsWithAvailabilities(Guid projectId)
        {
            if (projectId == Guid.Empty)
                return null;
            return EntitySet
                .Include(p => p.Project)
                .Include(p => p.Availabilities)
                .ThenInclude(a => a.Shift)
                .ThenInclude(s => s.Task)
                .Where(p => p.ProjectId == projectId)
                .ToListAsync();
        }
    }
}