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

        /// <summary>
        /// If the modelbuilder doesn't have any data of this enity, seed the data.
        /// </summary>
        /// <returns>A list with seeddata of this entity.</returns>
        public virtual List<T> Seed()
        {
            return new List<T>();
        }
    }
}
