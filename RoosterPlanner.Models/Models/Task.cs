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

        [Column(Order = 3)]
        public Guid CategoryId { get; set; }

        [Column(Order = 4)]
        [MaxLength(12)]
        public string Color { get; set; }

        [Column(Order = 5)]
        [MaxLength(128)]
        public string DocumentUri { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public List<ProjectTask> TaskProjects { get; set; }

        public List<Shift> Shifts { get; set; }

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
