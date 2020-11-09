using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class BaseSeed<T> where T : class, new()
    {
        protected ModelBuilder modelBuilder;

        //Constructor
        public BaseSeed(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public virtual List<T> Seed()
        {
            return new List<T>();
        }
    }
}
