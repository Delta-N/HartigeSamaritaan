using System;

namespace RoosterPlanner.Api.Models
{
    public class ScheduleViewModel
    {
        public PersonViewModel Person { get; set; }
        public int NumberOfTimesScheduledThisProject { get; set; }
        public Guid AvailabilityId { get; set; }
        public bool ScheduledThisDay { get; set; }
        public bool ScheduledThisShift { get; set; }
        public bool Preference { get; set; }
    }
}