using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class TaskSeed : BaseSeed<Task>
    {
        //Constructor
        public TaskSeed(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }
        /// <summary>
        /// If the modelbuilder doesn't have any data of this enity, seed the data.
        /// </summary>
        /// <returns>A list with seeddata of this entity.</returns>
        public override List<Task> Seed()
        {
            List<Task> tasks = new List<Task>
            {
                new(Guid.NewGuid())
                {
                    Name = "Chef", CategoryId = Guid.Parse("bd065d8a-c6f2-4ec5-84fd-92636f52f309"),
                    Color = "Blue",
                    Description = "Een leuke beschrijving van de werkzaamheden van een chef",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },

                new(Guid.NewGuid())
                {
                    Name = "Runner", CategoryId = Guid.Parse("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), Color = "Red",
                    Description = "Een leuke beschrijving van de werkzaamheden van een runner",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },

                new(Guid.NewGuid())
                {
                    Name = "Chauffeur", CategoryId = Guid.Parse("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"),
                    Color = "Yellow",
                    Description = "Een leuke beschrijving van de werkzaamheden van een chauffeur",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },

                new(Guid.NewGuid())
                {
                    Name = "Klusser", CategoryId = Guid.Parse("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), Color = "Green",
                    Description = "Een leuke beschrijving van de werkzaamheden van een klusser",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                }
            };

            modelBuilder.Entity<Task>().HasData(tasks.ToArray());
            return tasks;
        }
    }
}