using System;
using System.Collections.Generic;
using RoosterPlanner.Api.Models.EntityViewModels;
using RoosterPlanner.Models.Models.Enums;
namespace RoosterPlanner.Api.Models.HelperViewModels
{
    public class AvailabilityDataViewModel
    {
        /// <summary>
        /// Gets or sets the KnownAvailabiliteslist 
        /// </summary>
        public List<Schedule> KnownAvailabilities { get; set; }

        /// <summary>
        /// Gets or sets the ProjectTask list. 
        /// </summary>
        public List<TaskViewModel> ProjectTasks { get; set; }

        //Constructor
        public AvailabilityDataViewModel()
        {
            KnownAvailabilities = new List<Schedule>();
            ProjectTasks = new List<TaskViewModel>();
        }
    }
    /// <summary>
    /// DTO grouping a date and a Availabilitystatus
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// Gets or sets the Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the Status 
        /// </summary>
        public AvailabilityStatus Status { get; set; }
        //Constructor
        public Schedule(DateTime date, AvailabilityStatus status)
        {
            Date = date;
            Status = status;
        }
    }
}