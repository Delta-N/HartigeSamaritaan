using System.Collections.Generic;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class ParticipationSeed 
    {

        public static List<Participation> Seed()
        {
            List<Participation> participations = new List<Participation>();

            foreach (Project project in ProjectSeed.Seed())
            {
                Project projectObject = project;

                foreach (Person person1 in PersonSeed.Seed())
                {
                    Participation participation = new Participation(Helper.ConcatGuid(project.Id,person1.Id)) //using concat of projectID and personID
                    {
                        PersonId = person1.Id,
                        ProjectId = projectObject.Id,
                        Active = true,
                        Remark = "remark",
                        MaxWorkingHoursPerWeek = 12,
                    };
                    participations.Add(participation);
                }
            }



            return participations;
        }
    }
}