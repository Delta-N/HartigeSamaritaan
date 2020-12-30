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

        [Column(Order = 2)] public Guid? CategoryId { get; set; }

        [Column(Order = 3)] [MaxLength(12)] public string Color { get; set; }

        [Column(Order = 4)] 
        public Guid? InstructionId { get; set; }

        [ForeignKey("InstructionId")] 
        public Document Instruction { get; set; }

        [Column(Order = 5)] [MaxLength(256)] public string Description { get; set; }

        [ForeignKey("CategoryId")] public Category Category { get; set; }

        public List<ProjectTask> ProjectTasks { get; set; }

        public List<Shift> Shifts { get; set; }

        //Constructor
        public Task() : this(Guid.Empty)
        {
            ProjectTasks = new List<ProjectTask>();
            Shifts = new List<Shift>();
        }

        //Constructor
        public Task(Guid id) : base(id)
        {
            ProjectTasks = new List<ProjectTask>();
            Shifts = new List<Shift>();
        }
    }
}