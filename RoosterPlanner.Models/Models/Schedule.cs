using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Schedule : Entity
    {
        [Column(Order = 1)]
        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        [Column(Order = 2)]
        public DateTime Date { get; set; }

        //Constructor
        public Schedule() : base()
        {
        }
    }
}
