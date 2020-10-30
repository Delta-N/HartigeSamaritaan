using System;
using System.Collections.Generic;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ParticipationViewModel
    {
        public Guid Id { get; set; }
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
            return new ParticipationViewModel()
            {
                Id = participation.Id,
                Person = PersonViewModel.CreateVmFromPerson(participation.Person),
                Project = ProjectDetailsViewModel.CreateVm(participation.Project),
                MaxWorkingHoursPerWeek = participation.MaxWorkingHoursPerWeek,
                Availabilities = participation.Availabilities,
                WantsToWorkWith = participation.WantsToWorkWith
            };
        }

        public static Participation CreateParticipation(ParticipationViewModel participationViewModel)
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
            };
        }
    }
}