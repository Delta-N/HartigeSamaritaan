using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Manager : Entity
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

       

        //Constructor
        public Manager()
        {
        }
        //Constructor
        public Manager(Guid id) : base(id)
        {
        }
    }
}