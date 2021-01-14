using System.Collections.Generic;

namespace RoosterPlanner.Api.Models
{
    public class ScheduleDataViewModel
    {
        public List<ScheduleViewModel> Schedules { get; set; }
        public ShiftViewModel Shift { get; set; }
    }
}