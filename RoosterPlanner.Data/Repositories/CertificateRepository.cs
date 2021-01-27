using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface ICertificateRepository : IRepository<Certificate>
    {
        /// <summary>
        /// Get a certificated based on a Id including certificateType and person.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A task of a certificate.</returns>
        Task<Certificate> GetCertificateAsync(Guid id);
    }

    public class CertificateRepository : Repository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Get a certificated based on a Id including certificateType and person.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A task of a certificate.</returns>
        public Task<Certificate> GetCertificateAsync(Guid id)
        {
            if (id == Guid.Empty)
                return null;
            return EntitySet
                .AsNoTracking()
                .Include(c => c.CertificateType)
                .Include(c => c.Person)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}