using System;
using System.ComponentModel.DataAnnotations.Schema;
using RoosterPlanner.Models.Types;

namespace RoosterPlanner.Models
{
    public class Availability : Entity
    {
        [Column(Order = 1)]
        public Guid? ParticipationId { get; set; }

        [ForeignKey("ParticipationId")]
        public Participation Participation { get; set; }

        [Column(Order = 2)]
        public Guid ShiftId { get; set; }

        [ForeignKey("ShiftId")]
        public Shift Shift { get; set; }

        [Column(Order = 3)]
        public AvailibilityType Type { get; set; }

        [Column(Order = 4)]
        public bool Preference { get; set; }

        //Constructor
        public Availability() : this(Guid.Empty)
        {
            
        }

        //Constructor
        public Availability(Guid id) : base(id)
        {
        }
    }
}
