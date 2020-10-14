using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models.Models
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

        [Column(Order = 3)]
        public int MaxWorkingHoursPerWeek { get; set; }

        [Column(Order = 4)]
        public Guid FriendsId { get; set; }

        public List<Availability> Availabilities { get; set; }

        [ForeignKey("FriendsId")]
        public List<Participation> Friends { get; set; }


        //Constructor
        public Participation() : base()
        {
            Availabilities = new List<Availability>();
            Friends = new List<Participation>();
        }

        public Participation(Guid id) : base(id)
        {
            Availabilities = new List<Availability>();
            Friends = new List<Participation>();
        }
    }
}