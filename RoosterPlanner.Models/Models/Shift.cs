using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Shift : Entity
    {
        [Column(Order = 1)]
        public TimeSpan StartTime { get; set; }

        [Column(Order = 2)]
        public TimeSpan EndTime { get; set; }

        //Constructor
        public Shift() : base()
        {
        }
    }
}
