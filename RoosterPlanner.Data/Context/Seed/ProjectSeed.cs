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
            List<Project> projects = new List<Project>
            {
                new(Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe"))
                {
                    Name = "Voorburg 2020", Address = "Stationsplein 2", City = "Voorburg",
                    Description = "Leuk project in Voorburg", 
                    ParticipationStartDate = DateTime.Now.AddDays(1),
                    ParticipationEndDate = DateTime.Now.AddDays(29),
                    ProjectStartDate = DateTime.Now,
                    ProjectEndDate = DateTime.Now.AddDays(30),
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },

                new(Guid.Parse("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"))
                {
                    Name = "Den Haag 2018", Address = "Laan van Waalhaven 450", City = "Den Haag",
                    Description = "Leuk project in Den Haag",
                    ParticipationStartDate = DateTime.Now.AddDays(1),
                    ParticipationEndDate = DateTime.Now.AddDays(29),
                    ProjectStartDate = DateTime.Now,
                    ProjectEndDate = DateTime.Now.AddDays(30),
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                }
            };

            modelBuilder.Entity<Project>().HasData(projects.ToArray());

            return projects;
        }
    }
}