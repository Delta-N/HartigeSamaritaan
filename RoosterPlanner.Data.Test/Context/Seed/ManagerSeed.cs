using System;
using System.Collections.Generic;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class ManagerSeed
    {
        public static List<Manager> Seed()
        {
            List<Manager> managers = new List<Manager>();
            foreach (Project project in ProjectSeed.Seed())
            {
                Person one = Helper.ReturnRandomEntity(PersonSeed.Seed());
                Person two = Helper.ReturnRandomEntity(PersonSeed.Seed());
                while (one.Id == two.Id)
                    two = Helper.ReturnRandomEntity(PersonSeed.Seed());

                managers.Add(new Manager(Guid.NewGuid())
                {
                    PersonId = one.Id,
                    ProjectId = project.Id
                });
                managers.Add(new Manager(Guid.NewGuid())
                {
                    PersonId = two.Id,
                    ProjectId = project.Id
                });
            }

            return managers;
        }
    }
}