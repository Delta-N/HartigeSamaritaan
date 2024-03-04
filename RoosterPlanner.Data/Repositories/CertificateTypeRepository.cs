using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface ICertificateTypeRepository : IRepository<CertificateType>
    {
        /// <summary>
        /// Get all certificateTypes in the database.
        /// </summary>
        /// <returns>A task of a list of certificateTypes.</returns>
        Task<List<CertificateType>> GetAllCategoriesAsync();
    }

    public class CertificateTypeRepository : Repository<CertificateType>, ICertificateTypeRepository
    {
        public CertificateTypeRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Get all certificateTypes in the database.
        /// </summary>
        /// <returns>A task of a list of certificateTypes.</returns>
        public Task<List<CertificateType>> GetAllCategoriesAsync()
        {
            IQueryable<CertificateType> queryable = EntitySet
                .AsNoTracking();
            Task<List<CertificateType>> certificateTypes = queryable
                .OrderBy(c => c.Name)
                .ToListAsync();
            return certificateTypes;
        }
    }
}