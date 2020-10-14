using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Models;

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
            List<Category> categories = new List<Category>
            {
                new Category(Guid.NewGuid()) { Code = "KEUKEN", Name = "Keuken", LastEditBy = "System", LastEditDate = new DateTime(2019, 1, 22, 8, 1, 1) },
                new Category(Guid.NewGuid()) { Code = "BEDIENING", Name = "Bediening", LastEditBy = "System", LastEditDate = new DateTime(2019, 1, 18, 16, 55, 29) },
                new Category(Guid.NewGuid()) { Code = "LOGISTIEK", Name = "Logistiek", LastEditBy = "System", LastEditDate = new DateTime(2019, 1, 15, 2, 22, 55) }
            };

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
