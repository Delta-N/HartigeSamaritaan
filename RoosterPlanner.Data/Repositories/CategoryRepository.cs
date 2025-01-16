using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Get all categories in the database.
        /// </summary>
        /// <returns>A task of a list of categories.</returns>
        Task<List<Category>> GetAllCategoriesAsync();
    }

    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        //Constructor
        public CategoryRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Get all categories in the database.
        /// </summary>
        /// <returns>A task of a list of categories.</returns>
        public Task<List<Category>> GetAllCategoriesAsync()
        {
            IQueryable<Category> queryable = EntitySet
                .AsNoTracking();
            Task<List<Category>> categories = queryable.ToListAsync();
            return categories;
        }
    }
}