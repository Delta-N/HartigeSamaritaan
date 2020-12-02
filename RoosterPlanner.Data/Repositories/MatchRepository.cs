using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IMatchRepository : IRepository<Availability>
    {
    }

    public class MatchRepository : Repository<Availability>, IMatchRepository
    {
        //Constructor
        public MatchRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}
