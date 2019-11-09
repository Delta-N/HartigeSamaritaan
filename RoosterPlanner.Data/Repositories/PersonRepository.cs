using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
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
        public PersonRepository(RoosterPlannerContext dataContext, ILogger logger) : base(dataContext, logger)
        {
        }

        /// <summary>
        /// Returns a list of open projects.
        /// </summary>
        /// <returns>List of projects that are not closed.</returns>
        public Task<Person> GetPersonByOidAsync(Guid oid)
        {
            return this.EntitySet.Where(p => p.Oid == oid).FirstOrDefaultAsync();
        }
    }
}
