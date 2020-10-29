using System;
using System.Collections.Generic;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ParticipationViewModel
    {
        public Guid Id { get; set; }
        public Person Person { get; set; }
        public Project Project { get; set; }
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
            return new ParticipationViewModel()
            {
                Id = participation.Id,
                Person = participation.Person,
                Project = participation.Project,
                MaxWorkingHoursPerWeek = participation.MaxWorkingHoursPerWeek,
                Availabilities = participation.Availabilities,
                WantsToWorkWith = participation.WantsToWorkWith
            };
        }

        public static Participation CreateParticipation(ParticipationViewModel participationViewModel)
        {
            return new Participation(participationViewModel.Id)
            {
                Person = participationViewModel.Person,
                PersonId = participationViewModel.Person.Id,
                Project = participationViewModel.Project,
                ProjectId = participationViewModel.Project.Id,
                MaxWorkingHoursPerWeek = participationViewModel.MaxWorkingHoursPerWeek,
                Availabilities = participationViewModel.Availabilities,
                WantsToWorkWith = participationViewModel.WantsToWorkWith,
            };
        }
    }
}