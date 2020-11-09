using Microsoft.Extensions.Logging;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {

    }

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        //Constructor
        public CategoryRepository(RoosterPlannerContext dataContext) : base(dataContext)
        {
        }
    }
}
