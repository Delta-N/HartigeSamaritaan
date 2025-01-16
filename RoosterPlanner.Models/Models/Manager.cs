using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace RoosterPlanner.Models.Models
{
    public class Manager : Entity
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
