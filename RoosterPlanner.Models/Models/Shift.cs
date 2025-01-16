using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace RoosterPlanner.Models.Models
{
    public class Shift : Entity
    {
        /// <summary>
        /// Gets or sets the StartTime
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the EndTime 
        /// </summary>
        public TimeSpan EndTime { get; set; }

        /// <summary>
        /// Gets or sets the TypeName 
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the Availabilities 
        /// </summary>
        public List<Availability> Availabilities { get; set; }

        /// <summary>
        /// Gets or sets the TaskId 
        /// </summary>
        public Guid? TaskId { get; set; }

        /// <summary>
        /// Gets or sets the Task
        /// </summary>
        [ForeignKey("TaskId")]
        public Task Task { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId 
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the Project
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets the number of ParticipantsRequired 
        /// </summary>
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
