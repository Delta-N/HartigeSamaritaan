using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<Person> GetPersonByOidAsync(Guid oid);
    }

    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        //Constructor
        public PersonRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public async Task<Person> GetPersonByOidAsync(Guid oid)
        {
            Person person = await EntitySet
                .AsNoTracking()
                .Include(p => p.ProfilePicture)
                .Include(p => p.Certificates)
                .ThenInclude(c => c.CertificateType)
                .Where(p => p.Oid == oid)
                .FirstOrDefaultAsync();

            if (person?.Certificates == null) return person;
            foreach (Certificate personCertificate in person.Certificates.Where(personCertificate => personCertificate.CertificateType != null))
                personCertificate.CertificateType.Certificates = null;
            
            return person;
        }
    }
}