using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IParticipationRepository : IRepository<Participation>
    {
    }

    public class ParticipationRepository : Repository<Participation>, IParticipationRepository
    {
        //Constructor
        public ParticipationRepository(RoosterPlannerContext dataContext, ILogger logger) : base(dataContext, logger)
        {
        }
    }
}
