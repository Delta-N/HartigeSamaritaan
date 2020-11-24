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

        [Column(Order = 3, TypeName = "date")]
        public DateTime Date { get; set; }

        public List<Availability> Availabilities { get; set; }

        [Column(Order = 4)]
        public Guid? TaskId { get; set; }

        [ForeignKey("TaskId")]
        public Task Task { get; set; }

        [Column(Order = 5)]
        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        [Column(Order = 6)]
        public int ParticipantsRequired { get; set; }


        //Constructor
        public Shift() : base()
        {
            Availabilities = new List<Availability>();
        }

        public Shift(Guid id) : base(id)
        {
            Availabilities = new List<Availability>();
        }
    }
}