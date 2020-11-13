using System;
using System.Collections.Generic;
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
            List<Category> categories = new List<Category>
            {
                new Category(Guid.Parse("bd065d8a-c6f2-4ec5-84fd-92636f52f309"))
                {
                    Code = "KEUKEN", Name = "Keuken", LastEditBy = "System",
                    LastEditDate = new DateTime(2019, 1, 22, 8, 1, 1)
                },
                new Category(Guid.Parse("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"))
                {
                    Code = "BEDIENING", Name = "Bediening", LastEditBy = "System",
                    LastEditDate = new DateTime(2019, 1, 18, 16, 55, 29)
                },
                new Category(Guid.Parse("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"))
                {
                    Code = "LOGISTIEK", Name = "Logistiek", LastEditBy = "System",
                    LastEditDate = new DateTime(2019, 1, 15, 2, 22, 55)
                },
                new Category(Guid.Parse("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"))
                {
                    Code = "OVERIGE", Name = "Overige", LastEditBy = "System",
                    LastEditDate = new DateTime(2019, 1, 15, 2, 22, 55)
                }
            };

            this.modelBuilder.Entity<Category>().HasData(categories.ToArray());
            return categories;
        }
    }
}