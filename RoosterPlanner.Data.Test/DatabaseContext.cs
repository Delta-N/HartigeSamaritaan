using System.Linq;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Context;

namespace RoosterPlanner.Data
{
    public abstract class RoosterPlannerContextTest : RoosterPlannerContext
    {
        protected RoosterPlannerContextTest(DbContextOptions<RoosterPlannerContext> options) : base(options)
        {
            
        }

        public override int SaveChanges()
        {
            var affectedRows = base.SaveChanges();
            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                ChangeTracker.Entries()
                    .Where(e => e.Entity != null)
                    .ToList()
                    .ForEach(e => e.State = EntityState.Detached);
            }

            return affectedRows;
            
        }
    }
}
