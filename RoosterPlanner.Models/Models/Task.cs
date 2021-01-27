﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Task : Entity
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [Column(Order = 1)]
        [Required, MaxLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the CategoryId
        /// </summary>
        [Column(Order = 2)]
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the Color
        /// </summary>
        [Column(Order = 3)]
        [MaxLength(12)]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the InstructionId
        /// </summary>
        [Column(Order = 4)]
        public Guid? InstructionId { get; set; }

        /// <summary>
        /// Gets or sets the Instruction 
        /// </summary>
        [ForeignKey("InstructionId")]
        public Document Instruction { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        [Column(Order = 5)]
        [MaxLength(256)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Category
        /// </summary>
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        /// <summary>
        /// Gets or sets the ProjectTasks
        /// </summary>
        public List<ProjectTask> ProjectTasks { get; set; }

        /// <summary>
        /// Gets or sets the Requirements
        /// </summary>
        public List<Requirement> Requirements { get; set; }

        /// <summary>
        /// Gets or sets the Shifts
        /// </summary>
        public List<Shift> Shifts { get; set; }

        //Constructor
        public Task() : this(Guid.Empty)
        {
            ProjectTasks = new List<ProjectTask>();
            Shifts = new List<Shift>();
            Requirements = new List<Requirement>();
        }

        //Constructor
        public Task(Guid id) : base(id)
        {
            ProjectTasks = new List<ProjectTask>();
            Shifts = new List<Shift>();
            Requirements = new List<Requirement>();
        }
    }
}