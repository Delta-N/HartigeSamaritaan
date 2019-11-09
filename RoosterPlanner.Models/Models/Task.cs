using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Task : Entity
    {
        [Column(Order = 1)]
        [Required, MaxLength(64)]
        public string Name { get; set; }

        [Column(Order = 2)]
        public DateTime? DeletedDateTime { get; set; }

        public List<ProjectTask> TaskProjects { get; set; }

        //Constructor
        public Task() : this(Guid.Empty)
        {
        }

        //Constructor
        public Task(Guid id) : base(id)
        {
            this.TaskProjects = new List<ProjectTask>();
        }
    }
}
