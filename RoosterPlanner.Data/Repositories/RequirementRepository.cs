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
        Task<Requirement> GetRequirementAsync(Guid id);
        Task<Requirement> GetRequirementAsync(Guid taskId, Guid certificateTypeId);
    }

    public class RequirementRepository : Repository<Requirement>, IRequirementRepository
    {
        public RequirementRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Task<Requirement> GetRequirementAsync(Guid taskId, Guid certificateTypeId)
        {
            if (taskId == Guid.Empty || certificateTypeId == Guid.Empty)
                return null;
            return EntitySet
                .AsNoTracking()
                .Where(r => r.CertificateTypeId == certificateTypeId && r.TaskId == taskId)
                .FirstOrDefaultAsync();
        }

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