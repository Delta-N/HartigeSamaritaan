using System;
using System.Collections.Generic;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class TaskSeed
    {
        public static List<Task> Seed()
        {
            List<Task> tasks = new List<Task>
            {
                new(Guid.Parse("7c1046da-c3b1-498e-bd78-08eed4a53a56"))
                {
                    Name = "Chef", CategoryId = Guid.Parse("bd065d8a-c6f2-4ec5-84fd-92636f52f309"),
                    Color = "Blue",
                    Description = "Een leuke beschrijving van de werkzaamheden van een chef",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now,
                    InstructionId = Guid.Parse("23fe35ed-3333-42a7-86d6-136803b5b42b")
                },

                new(Guid.Parse("a9863d58-07c9-4f23-ae87-ee96d0d3f3d4"))
                {
                    Name = "Runner", CategoryId = Guid.Parse("4c23384e-76bd-4957-a7e7-2ba9bd44dc00"), Color = "Red",
                    Description = "Een leuke beschrijving van de werkzaamheden van een runner",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now,
                    InstructionId = Guid.Parse("23fe35ed-3333-42a7-86d6-136803b5b42b")
                },

                new(Guid.Parse("1316726a-a651-4cef-800d-a74746ca3d74"))
                {
                    Name = "Chauffeur", CategoryId = Guid.Parse("c547a3d4-f726-4db8-bd40-8c27c5e8cb05"),
                    Color = "Yellow",
                    Description = "Een leuke beschrijving van de werkzaamheden van een chauffeur",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now,
                    InstructionId = Guid.Parse("23fe35ed-3333-42a7-86d6-136803b5b42b")
                },

                new(Guid.Parse("e823f81f-5fad-46b9-8301-355e0b24856e"))
                {
                    Name = "Klusser", CategoryId = Guid.Parse("ba35a8ac-5f2a-4e67-9146-63f62ade6ad2"), Color = "Green",
                    Description = "Een leuke beschrijving van de werkzaamheden van een klusser",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now,
                    InstructionId = Guid.Parse("23fe35ed-3333-42a7-86d6-136803b5b42b")
                }
            };

            return tasks;
        }
    }
}