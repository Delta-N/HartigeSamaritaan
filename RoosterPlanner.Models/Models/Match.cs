using RoosterPlanner.Models.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Match : Entity
    {
        [Column(Order = 1)]
        public Guid ParticipationId { get; set; }

        [ForeignKey("ParticipationId")]
        public Participation Participation { get; set; }

        [Column(Order = 2)]
        public Guid ShiftId { get; set; }

        [ForeignKey("ShiftId")]
        public Shift Shift { get; set; }

        public MatchType Type { get; set; }

        //Constructor
        public Match() : base()
        {
        }
        //Constructor
        public Match(Guid id) : base(id)
        {
        }
    }
}
