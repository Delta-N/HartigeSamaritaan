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
        Task<Certificate> GetCertificateAsync(Guid id);
    }

    public class CertificateRepository : Repository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(DbContext dataContext) : base(dataContext)
        {
        }

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