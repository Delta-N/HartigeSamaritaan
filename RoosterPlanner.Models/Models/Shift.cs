using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Shift : Entity
    {
        /// <summary>
        /// Gets or sets the StartTime
        /// </summary>
        [Column(Order = 1)]
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the EndTime 
        /// </summary>
        [Column(Order = 2)]
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Gets or sets the TypeName 
        /// </summary>
        [Column(Order = 3, TypeName = "date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the Availabilities 
        /// </summary>
        public List<Availability> Availabilities { get; set; }

        /// <summary>
        /// Gets or sets the TaskId 
        /// </summary>
        [Column(Order = 4)]
        public Guid? TaskId { get; set; }

        /// <summary>
        /// Gets or sets the Task
        /// </summary>
        [ForeignKey("TaskId")]
        public Task Task { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId 
        /// </summary>
        [Column(Order = 5)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the Project
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets the number of ParticipantsRequired 
        /// </summary>
        [Column(Order = 6)]
        public int ParticipantsRequired { get; set; }


        //Constructor
        public Shift()
        {
            Availabilities = new List<Availability>();
        }
        //constructor
        public Shift(Guid id) : base(id)
        {
            Availabilities = new List<Availability>();
        }
    }
}