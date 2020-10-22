using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class ProjectTask : Entity
    {
        [Column(Order = 0)]
        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [Column(Order = 1)]
        public Guid TaskId { get; set; }

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
