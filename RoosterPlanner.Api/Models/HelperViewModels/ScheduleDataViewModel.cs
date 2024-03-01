using System.Collections.Generic;
using RoosterPlanner.Api.Models.EntityViewModels;
namespace RoosterPlanner.Api.Models.HelperViewModels
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