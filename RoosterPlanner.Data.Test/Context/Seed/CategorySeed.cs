using System;
using System.Collections.Generic;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class CategorySeed
    {
        public static List<Category> Seed()
        {
            List<Category> categories = new List<Category>
            {
                new(Guid.Parse("bd065d8a-c6f2-4ec5-84fd-92636f52f309"))
                {
                    Code = "KEUKEN", Name = "Keuken", 
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },
                new(Guid.Parse("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"))
                {
                    Code = "BEDIENING", Name = "Bediening", 
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },
                new(Guid.Parse("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"))
                {
                    Code = "LOGISTIEK", Name = "Logistiek", 
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },
                new(Guid.Parse("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"))
                {
                    Code = "OVERIGE", Name = "Overige", 
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                }
            };
            return categories;
        }
    }
}