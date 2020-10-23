using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class ProjectSeed : BaseSeed<Project>
    {
        //Constructor
        public ProjectSeed(ModelBuilder modelBuilder) : base(modelBuilder)
        {
            
        }

        public override List<Project> Seed()
        {
            var projects = new List<Project>()
            {
                new Project(Guid.NewGuid())
                {
                    Name = "Voorburg 2020", Address = "Stationsplein 2", City = "Voorburg",
                    Description = "Leuk project in Voorburg", StartDate = new DateTime(2020, 10, 14),
                    EndDate = new DateTime(2020, 10, 15)
                },

                new Project(Guid.NewGuid())
                {
                    Name = "Den Haag 2018", Address = "Laan van Waalhaven 450", City = "Den Haag",
                    Description = "Leuk project in Den Haag", StartDate = new DateTime(2018, 10, 14),
                    EndDate = new DateTime(2018, 10, 15)
                }
            };
                
            

            modelBuilder.Entity<Project>().HasData(projects.ToArray());

            return projects;
        }
    }
}