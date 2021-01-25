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
        /// <summary>
        /// Gets a person based on the OID including certificates and a profile picture
        /// </summary>
        /// <param name="oid"></param>
        /// <returns>Task of a Person</returns>
        Task<Person> GetPersonByOidAsync(Guid oid);
    }

    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        //Constructor
        public PersonRepository(DbContext dataContext) : base(dataContext)
        {
        }
        
        /// <summary>
        /// Gets a person based on the OID including certificates and a profile picture
        /// </summary>
        /// <param name="oid"></param>
        /// <returns>Task of a Person</returns>
        public async Task<Person> GetPersonByOidAsync(Guid oid)
        {
            if (oid == Guid.Empty)
                return null;
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