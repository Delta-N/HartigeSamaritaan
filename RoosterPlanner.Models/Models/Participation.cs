using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Participation : Entity
    {
        /// <summary>
        /// Gets or sets the PersonId
        /// </summary>
        [Column(Order = 1)]
        public Guid PersonId { get; set; }

        /// <summary>
        /// Gets or sets the Person 
        /// </summary>
        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        /// <summary>
        /// Gets or sets the ProjectId 
        /// </summary>
        [Column(Order = 2)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the Project 
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets the MaxWorkingHoursPerWeek 
        /// </summary>
        [Column(Order = 3)]
        public int MaxWorkingHoursPerWeek { get; set; }

        /// <summary>
        /// Gets or sets the Active
        /// </summary>
        [Column(Order = 4)]
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets the Remark 
        /// </summary>
        [Column(Order = 5)]
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