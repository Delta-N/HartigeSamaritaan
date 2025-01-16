using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RoosterPlanner.Models.Models
{
    public class Category : Entity
    {
        /// <summary>
        /// Gets or sets the Code
        /// </summary>

        [Required, MaxLength(10)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        [Required, MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Tasks
        /// </summary>
        public List<Task> Tasks { get; set; }


        //Constructor
        public Category() : this(Guid.Empty)
        {
            Tasks=new List<Task>();
        }

        //Constructor
        public Category(Guid id) : base(id)
        {
            Tasks=new List<Task>();
        }
    }
}
