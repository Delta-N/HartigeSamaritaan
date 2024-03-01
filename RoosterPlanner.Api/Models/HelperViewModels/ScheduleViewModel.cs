using System;
using System.Collections.Generic;
using RoosterPlanner.Api.Models.EntityViewModels;
namespace RoosterPlanner.Api.Models.HelperViewModels
{
    public class ScheduleViewModel
    {
        /// <summary>
        /// Gets or sets the Person
        /// </summary>
        public PersonViewModel Person { get; set; }

        /// <summary>
        /// Gets or sets the NumberOfTimeScheduledThisProject 
        /// </summary>
        public int NumberOfTimesScheduledThisProject { get; set; }

        /// <summary>
        /// Gets or sets the AvailabilityId 
        /// </summary>
        public Guid AvailabilityId { get; set; }

        /// <summary>
        /// Gets or sets the ScheduledThisDay 
        /// </summary>
        public bool ScheduledThisDay { get; set; }

        /// <summary>
        /// Gets or sets ScheduledThisShift 
        /// </summary>
        public bool ScheduledThisShift { get; set; }

        /// <summary>
        /// Gets or sets the Preference
        /// </summary>
        public bool Preference { get; set; }

        /// <summary>
        /// Gets or sets the Availabilities 
        /// </summary>
        public List<AvailabilityViewModel> Availabilities { get; set; }

        /// <summary>
        /// Gets or sets the number of hours a person is scheduled this week
        /// </summary>
        public double HoursScheduledThisWeek { get; set; }

        /// <summary>
        /// Gets or sets the number of hours a person is willing to work this week. 
        /// </summary>
        public double Employability { get; set; }
    }
}