using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface ICertificateTypeRepository : IRepository<CertificateType>
    {
        Task<List<CertificateType>> GetAllCategoriesAsync();
    }

    public class CertificateTypeRepository : Repository<CertificateType>, ICertificateTypeRepository
    {
        public CertificateTypeRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Task<List<CertificateType>> GetAllCategoriesAsync()
        {
            IQueryable<CertificateType> queryable = EntitySet
                .AsNoTracking();
            Task<List<CertificateType>> certificateTypes = queryable.ToListAsync();
            return certificateTypes;
        }
    }
}