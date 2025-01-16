using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RoosterPlanner.Models.Models
{
    public class Participation : Entity
    {
        /// <summary>
        /// Gets or sets the PersonId
        /// </summary>
        public Guid PersonId { get; set; }

        /// <summary>
        /// Gets or sets the Person 
        /// </summary>
        [ForeignKey("PersonId")]
        public Person Person { get; set; }

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
        /// Gets or sets the MaxWorkingHoursPerWeek 
        /// </summary>
        public int MaxWorkingHoursPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the Active
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets the Remark 
        /// </summary>
        [MaxLength(256)]
        public string Remark { get; set; }

        /// <summary>
        /// Gets or sets the Availabilities
        /// </summary>
        public List<Availability> Availabilities { get; set; }

        //Constructor
        public Participation()
        {
            Availabilities = new List<Availability>();
        }

        //Constructor
        public Participation(Guid id) : base(id)
        {
            Availabilities = new List<Availability>();
        }
    }
}
