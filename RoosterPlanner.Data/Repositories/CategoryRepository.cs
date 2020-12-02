using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetAll();
    }

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        //Constructor
        public CategoryRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Task<List<Category>> GetAll()
        {
            IQueryable<Category> queryable = EntitySet.AsNoTracking().AsQueryable();
            Task<List<Category>> categories = queryable.ToListAsync();
            return categories;
        }
    }
}