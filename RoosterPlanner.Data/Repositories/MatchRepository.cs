using Microsoft.Extensions.Logging;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IMatchRepository : IRepository<Availability>
    {
    }

    public class MatchRepository : Repository<Availability>, IMatchRepository
    {
        //Constructor
        public MatchRepository(RoosterPlannerContext dataContext) : base(dataContext)
        {
        }
    }
}
