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
        public Task<Person> GetPersonByOidAsync(Guid oid)
        {
            return EntitySet
                .Include(p=>p.ProfilePicture)
                .Where(p => p.Oid == oid)
                .FirstOrDefaultAsync();
        }
    }
}
