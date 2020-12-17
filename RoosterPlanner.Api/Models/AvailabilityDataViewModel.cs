using System;
using System.Collections.Generic;
using RoosterPlanner.Models.Models.Enums;

namespace RoosterPlanner.Api.Models.Constants
{
    public class AvailabilityDataViewModel
    {
        public List<Schedule> KnownAvailabilities { get; set; }
        public List<TaskViewModel> ProjectTasks { get; set; }

        public AvailabilityDataViewModel()
        {
            KnownAvailabilities = new List<Schedule>();
            ProjectTasks = new List<TaskViewModel>();
        }
    }

    public class Schedule
    {
        public DateTime Date { get; set; }
        public AvailabilityStatus Status { get; set; }

        public Schedule(DateTime date, AvailabilityStatus status)
        {
            Date = date;
            Status = status;
        }
    }
}