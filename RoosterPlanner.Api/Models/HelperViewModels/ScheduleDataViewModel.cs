using System.Collections.Generic;

namespace RoosterPlanner.Api.Models
{
    /// <summary>
    /// DTO for transfering Schedules of a shift
    /// Used on 'plan-shift page.
    /// </summary>
    public class ScheduleDataViewModel
    {
        /// <summary>
        /// Gets or sets the Schedules 
        /// </summary>
        public List<ScheduleViewModel> Schedules { get; set; }

        /// <summary>
        /// Gets or sets the Shift. 
        /// </summary>
        public ShiftViewModel Shift { get; set; }
    }
}