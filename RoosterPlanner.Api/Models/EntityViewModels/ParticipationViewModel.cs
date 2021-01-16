﻿using System.Collections.Generic;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ParticipationViewModel : EntityViewModel
    {
        public PersonViewModel Person { get; set; }
        public ProjectDetailsViewModel Project { get; set; }
        public int MaxWorkingHoursPerWeek { get; set; }
        public string Remark { get; set; }
        public List<Availability> Availabilities { get; set; }

        public ParticipationViewModel()
        {
            Availabilities = new List<Availability>();
        }

        public bool Active { get; set; }

        public static ParticipationViewModel CreateVm(Participation participation)
        {
            if (participation == null) 
                return null;
            ParticipationViewModel vm = new ParticipationViewModel
            {
                Id = participation.Id,
                MaxWorkingHoursPerWeek = participation.MaxWorkingHoursPerWeek,
                Availabilities = participation.Availabilities,
                Remark = participation.Remark,
                Active = participation.Active,
                LastEditDate = participation.LastEditDate,
                LastEditBy = participation.LastEditBy,
                RowVersion = participation.RowVersion
            };
            if(participation.Person!=null)
                vm.Person=PersonViewModel.CreateVmFromPerson(participation.Person);
            if(participation.Project!=null)
                vm.Project=ProjectDetailsViewModel.CreateVm(participation.Project);
            return vm;

        }

        public static Participation CreateParticipation(ParticipationViewModel participationViewModel)
        {
            if (participationViewModel == null) 
                return null;
            
            Participation participation =  new Participation(participationViewModel.Id)
            {
                MaxWorkingHoursPerWeek = participationViewModel.MaxWorkingHoursPerWeek,
                Availabilities = participationViewModel.Availabilities,
                Remark = participationViewModel.Remark,
                Active = participationViewModel.Active,
                LastEditDate = participationViewModel.LastEditDate,
                LastEditBy = participationViewModel.LastEditBy,
                RowVersion = participationViewModel.RowVersion
            };

            if (participationViewModel.Person != null)
            {
                participation.Person = PersonViewModel.CreatePerson(participationViewModel.Person);
                participation.PersonId = participationViewModel.Person.Id;
            }

            if (participationViewModel.Project == null) 
                return participation;
            participation.Project = ProjectDetailsViewModel.CreateProject(participationViewModel.Project);
            participation.ProjectId = participationViewModel.Project.Id;

            return participation;

        }
    }
}