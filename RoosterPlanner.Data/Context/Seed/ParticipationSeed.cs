using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class ParticipationSeed : BaseSeed<Participation>
    {
        public ParticipationSeed(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        public override List<Participation> Seed()
        {
            var participations = new List<Participation>()
            {
                new Participation(Guid.Parse("66e971cf-16f2-4521-befb-aaca981f642f"))
                {
                    PersonId = Guid.Parse("25E5B0E6-82EF-45FE-BBDE-EF76021EC531"),
                    ProjectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe"),
                    MaxWorkingHoursPerWeek = 12
                },
                new Participation(Guid.Parse("541310c7-ffec-43f5-81a7-7b2c07f9ce81"))
                {
                    PersonId = Guid.Parse("7F66FC12-B1C0-481F-851B-3CC1F65FD20E"),
                    ProjectId = Guid.Parse("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"),
                    MaxWorkingHoursPerWeek = 40
                }
            };

            modelBuilder.Entity<Participation>().HasData(participations.ToArray());

            return participations;
        }
    }
}