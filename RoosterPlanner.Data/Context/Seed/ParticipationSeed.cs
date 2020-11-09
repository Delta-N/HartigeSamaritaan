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
            Person one = new Person(Guid.Parse("25E5B0E6-82EF-45FE-BBDE-EF76021EC531"))
            {
                Oid = Guid.Parse("B691F9F7-C404-4D52-A34F-C90702CA7138"), firstName = "Grace",
                LastEditBy = "System", LastEditDate = new DateTime(2019, 1, 1, 12, 34, 28)
            };

            Person two = new Person(Guid.Parse("7F66FC12-B1C0-481F-851B-3CC1F65FD20E"))
            {
                Oid = Guid.Parse("E2A94901-6942-4CFB-83FA-60343C0DE219"), firstName = "John", LastEditBy = "System",
                LastEditDate = new DateTime(2019, 1, 2, 12, 45, 1)
            };

            Project three = new Project(Guid.NewGuid())
            {
                Name = "Voorburg 2020", Address = "Stationsplein 2", City = "Voorburg",
                Description = "Leuk project in Voorburg", StartDate = new DateTime(2020, 10, 14),
                EndDate = new DateTime(2020, 10, 15)
            };

            Project four = new Project(Guid.NewGuid())
            {
                Name = "Den Haag 2018", Address = "Laan van Waalhaven 450", City = "Den Haag",
                Description = "Leuk project in Den Haag", StartDate = new DateTime(2018, 10, 14),
                EndDate = new DateTime(2018, 10, 15)
            };

            var participations = new List<Participation>()
            {
                new Participation()
                {
                    Person = one,
                    PersonId = one.Id,
                    Project = three,
                    ProjectId = three.Id,
                    MaxWorkingHoursPerWeek = 12
                },
                new Participation()
                {
                    Person = two,
                    PersonId = two.Id,
                    Project = four,
                    ProjectId = four.Id,
                    MaxWorkingHoursPerWeek = 40
                }
            };

            modelBuilder.Entity<Participation>().HasData(participations.ToArray());

            return participations;
        }
    }
}