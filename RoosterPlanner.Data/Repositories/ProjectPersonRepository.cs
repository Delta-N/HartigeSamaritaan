using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IProjectPersonRepository : IRepository<ProjectPerson>
    {

    }

    public class ProjectPersonRepository : Repository<ProjectPerson>, IProjectPersonRepository
    {
        //Constructor
        public ProjectPersonRepository(RoosterPlannerContext dataContext, ILogger logger) : base(dataContext, logger)
        {
        }
    }
}
