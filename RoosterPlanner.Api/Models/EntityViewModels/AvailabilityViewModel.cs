using System;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Models.Models.Types;
namespace RoosterPlanner.Api.Models.EntityViewModels
{
    public class AvailabilityViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the ParticipationId  
        /// </summary>
        public Guid? ParticipationId { get; set; }

        /// <summary>
        /// Gets or sets the Participation 
        /// </summary>
        public ParticipationViewModel Participation { get; set; }

        /// <summary>
        /// Gets or sets the ShiftId 
        /// </summary>
        public Guid ShiftId { get; set; }

        /// <summary>
        /// Gets or sets the Shift 
        /// </summary>
        public ShiftViewModel Shift { get; set; }

        /// <summary>
        /// Gets or sets the Type 
        /// </summary>
        public AvailibilityType Type { get; set; }

        /// <summary>
        /// Gets or sets the Preference 
        /// </summary>
        public bool Preference { get; set; }

        /// <summary>
        /// Gets or sets the PushEmailSend 
        /// </summary>
        public bool PushEmailSend { get; set; }

        /// <summary>
        /// Creates a ViewModel from a Availability
        /// </summary>
        /// <param name="availability"></param>
        /// <returns></returns>
        public static AvailabilityViewModel CreateVm(Availability availability)
        {
            if (availability == null)
                return null;

            AvailabilityViewModel vm = new AvailabilityViewModel
            {
                Id = availability.Id,
                ParticipationId = availability.ParticipationId,
                ShiftId = availability.ShiftId,
                Type = availability.Type,
                Preference = availability.Preference,
                PushEmailSend = availability.PushEmailSend,
                LastEditDate = availability.LastEditDate,
                LastEditBy = availability.LastEditBy,
                RowVersion = availability.RowVersion
            };
            if (availability.Participation != null)
                vm.Participation = ParticipationViewModel.CreateVm(availability.Participation);
            if (availability.Shift != null)
                vm.Shift = ShiftViewModel.CreateVm(availability.Shift);
            return vm;
        }

        /// <summary>
        /// Creates a Availability from a ViewModel
        /// </summary>
        /// <param name="availabilityViewModel"></param>
        /// <returns></returns>
        public static Availability CreateAvailability(AvailabilityViewModel availabilityViewModel)
        {
            if (availabilityViewModel == null)
                return null;
            Availability availability = new Availability(availabilityViewModel.Id)
            {
                ParticipationId = availabilityViewModel.ParticipationId,
                ShiftId = availabilityViewModel.ShiftId,
                Type = availabilityViewModel.Type,
                Preference = availabilityViewModel.Preference,
                PushEmailSend = availabilityViewModel.PushEmailSend,
                LastEditDate = availabilityViewModel.LastEditDate,
                LastEditBy = availabilityViewModel.LastEditBy,
                RowVersion = availabilityViewModel.RowVersion
            };
            if (availabilityViewModel.Participation != null)
                availability.Participation =
                    ParticipationViewModel.CreateParticipation(availabilityViewModel.Participation);
            if (availabilityViewModel.Shift != null)
                availability.Shift = ShiftViewModel.CreateShift(availabilityViewModel.Shift);

            return availability;
        }
    }
}
