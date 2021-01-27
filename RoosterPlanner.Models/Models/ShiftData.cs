using System;
using System.Collections.Generic;

namespace RoosterPlanner.Models.Models
{
    public class ShiftData
    {
        /// <summary>
        /// Gets or sets the dinstinct Tasks of a project
        /// </summary>
        public List<Task> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the distinct Dates of a project 
        /// </summary>
        public List<DateTime> Dates { get; set; }

        /// <summary>
        /// Gets or sets the distinct StartTimes of a project 
        /// </summary>
        public List<string> StartTimes { get; set; }

        /// <summary>
        /// Gets or sets the distinct EndTimes of a project 
        /// </summary>
        public List<string> EndTimes { get; set; }

        /// <summary>
        /// Gets or sets the distinct number of participants of a project 
        /// </summary>
        public List<int> ParticipantsRequired { get; set; }
    }
}