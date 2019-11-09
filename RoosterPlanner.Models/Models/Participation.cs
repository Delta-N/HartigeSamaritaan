using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Participation : Entity
    {
        [Column(Order = 1)]
        public Guid PersonId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        [Column(Order = 2)]
        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public List<Match> Matches { get; set; }

        //Constructor
        public Participation() : base()
        {
            Matches = new List<Match>();
        }
        public Participation(Guid id) : base(id)
        {
            Matches = new List<Match>();
        }
    }
}
