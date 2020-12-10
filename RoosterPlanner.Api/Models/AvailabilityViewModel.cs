using System;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Types;

namespace RoosterPlanner.Api.Models
{
    public class AvailabilityViewModel : EntityViewModel
    {
        public Guid? ParticipationId { get; set; }
        public ParticipationViewModel Participation { get; set; }
        public Guid ShiftId { get; set; }
        public ShiftViewModel Shift { get; set; }
        public AvailibilityType Type { get; set; }
        public bool Preference { get; set; }

        public static AvailabilityViewModel CreateVm(Availability availability)
        {
            if (availability == null)
                return null;

            AvailabilityViewModel vm = new AvailabilityViewModel
            {
                Id = availability.Id,
                ParticipationId = availability.ParticipationId,
                ShiftId = availability.ShiftId,
                Participation = ParticipationViewModel.CreateVm(availability.Participation),
                Shift = ShiftViewModel.CreateVm(availability.Shift),
                Type = availability.Type,
                Preference = availability.Preference,

                LastEditDate = availability.LastEditDate,
                LastEditBy = availability.LastEditBy,
                RowVersion = availability.RowVersion
            };
            return vm;
        }

        public static Availability CreateAvailability(AvailabilityViewModel availabilityViewModel)
        {
            if (availabilityViewModel == null)
                return null;
            return new Availability(availabilityViewModel.Id)
            {
                Participation = ParticipationViewModel.CreateParticipation(availabilityViewModel.Participation),
                Shift = ShiftViewModel.CreateShift(availabilityViewModel.Shift),
                ParticipationId = availabilityViewModel.ParticipationId,
                ShiftId = availabilityViewModel.ShiftId,
                Type = availabilityViewModel.Type,
                Preference = availabilityViewModel.Preference,

                LastEditDate = availabilityViewModel.LastEditDate,
                LastEditBy = availabilityViewModel.LastEditBy,
                RowVersion = availabilityViewModel.RowVersion
            };
        }
    }
}