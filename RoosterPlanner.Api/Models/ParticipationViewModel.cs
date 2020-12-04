using System;
using System.Collections.Generic;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ParticipationViewModel : EntityViewModel
    {
        public PersonViewModel Person { get; set; }
        public ProjectDetailsViewModel Project { get; set; }
        public int MaxWorkingHoursPerWeek { get; set; }
        public List<Availability> Availabilities { get; set; }
        public List<Collaboration> WantsToWorkWith { get; set; }

        public ParticipationViewModel()
        {
            Availabilities = new List<Availability>();
            WantsToWorkWith = new List<Collaboration>();
        }

        public static ParticipationViewModel CreateVm(Participation participation)
        {
            if (participation != null)
            {
                return new ParticipationViewModel
                {
                    Id = participation.Id,
                    Person = PersonViewModel.CreateVmFromPerson(participation.Person),
                    Project = ProjectDetailsViewModel.CreateVm(participation.Project),
                    MaxWorkingHoursPerWeek = participation.MaxWorkingHoursPerWeek,
                    Availabilities = participation.Availabilities,
                    WantsToWorkWith = participation.WantsToWorkWith,
                    LastEditDate = participation.LastEditDate,
                    LastEditBy = participation.LastEditBy,
                    RowVersion = participation.RowVersion
                };
            }

            return null;
        }

        public static Participation CreateParticipation(ParticipationViewModel participationViewModel)
        {
            if (participationViewModel != null)
            {
                return new Participation(participationViewModel.Id)
                {
                    Person = PersonViewModel.CreatePerson(participationViewModel.Person),
                    PersonId = participationViewModel.Person.Id,
                    Project = ProjectDetailsViewModel.CreateProject(participationViewModel.Project),
                    ProjectId = participationViewModel.Project.Id,
                    MaxWorkingHoursPerWeek = participationViewModel.MaxWorkingHoursPerWeek,
                    Availabilities = participationViewModel.Availabilities,
                    WantsToWorkWith = participationViewModel.WantsToWorkWith,
                    LastEditDate = participationViewModel.LastEditDate,
                    LastEditBy = participationViewModel.LastEditBy,
                    RowVersion = participationViewModel.RowVersion
                };
            }

            return null;
        }
    }
}