using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IRequirementRepository : IRepository<Requirement>
    {
        /// <summary>
        /// Get a requirement based on an id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A task of a requirement</returns>
        Task<Requirement> GetRequirementAsync(Guid id);

        /// <summary>
        /// Get requirement based on taskId and certificateId
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="certificateTypeId"></param>
        /// <returns>A task of a requirement</returns>
        Task<Requirement> GetRequirementAsync(Guid taskId, Guid certificateTypeId);
    }

    public class RequirementRepository : Repository<Requirement>, IRequirementRepository
    {
        public RequirementRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Get requirement based on taskId and certificateId
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="certificateTypeId"></param>
        /// <returns>A task of a requirement</returns>
        public Task<Requirement> GetRequirementAsync(Guid taskId, Guid certificateTypeId)
        {
            if (taskId == Guid.Empty || certificateTypeId == Guid.Empty)
                return null;
            return EntitySet
                .AsNoTracking()
                .Where(r => r.CertificateTypeId == certificateTypeId && r.TaskId == taskId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get a requirement based on an id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A task of a requirement</returns>
        public async Task<Requirement> GetRequirementAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;
            Requirement requirement = await EntitySet
                .AsNoTracking()
                .Include(r => r.CertificateType)
                .Include(r => r.Task)
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();

            requirement.Task.Requirements = null;
            requirement.CertificateType.Requirements = null;

            return requirement;
        }
    }
}