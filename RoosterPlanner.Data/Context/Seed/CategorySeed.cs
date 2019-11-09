using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class CategorySeed : BaseSeed<Category>
    {
        //Constructor
        public CategorySeed(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        public override List<Category> Seed()
        {
            List<Category> categories = new List<Category>();
            categories.Add(new Category(Guid.NewGuid()) { Code = "KEUKEN", Name = "Keuken", LastEditBy = "System", LastEditDate = DateTime.UtcNow });
            categories.Add(new Category(Guid.NewGuid()) { Code = "BEDIENING", Name = "Bediening", LastEditBy = "System", LastEditDate = DateTime.UtcNow });
            categories.Add(new Category(Guid.NewGuid()) { Code = "LOGISTIEK", Name = "Logistiek", LastEditBy = "System", LastEditDate = DateTime.UtcNow });

            this.modelBuilder.Entity<Category>().HasData(categories.ToArray());
            //List<Category> currentList = entitySet.Take(50).ToList();
            //if (currentList.Count < categories.Count)
            //{
                
            //    foreach (Category item in categories)
            //    {
            //        Category tmpItem = entitySet.FirstOrDefault(x => x.Code == item.Code);
            //        if (tmpItem == null)
            //            entitySet.Add(item);
            //    }
            //}

            return categories;
        }
    }
}
