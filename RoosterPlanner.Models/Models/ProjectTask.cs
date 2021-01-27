using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class ProjectTask : Entity
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        [Column(Order = 0)]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the Project 
        /// </summary>
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        /// <summary>
        /// Gets or sets the TaskId 
        /// </summary>
        [Column(Order = 1)]
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the Task 
        /// </summary>
        [ForeignKey("TaskId")]
        public Task Task { get; set; }

        //Constructor
        public ProjectTask() : base()
        {
        }

        public ProjectTask(Guid id) : base(id)
        {
        }
    }
}