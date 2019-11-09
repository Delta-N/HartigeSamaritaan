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
    public interface IMatchRepository : IRepository<Models.Match>
    {
    }

    public class MatchRepository : Repository<Models.Match>, IMatchRepository
    {
        //Constructor
        public MatchRepository(RoosterPlannerContext dataContext, ILogger logger) : base(dataContext, logger)
        {
        }
    }
}
