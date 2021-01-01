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

        [Column(Order = 3)]
        public int MaxWorkingHoursPerWeek { get; set; }

        [Column(Order = 4)] 
        public bool Active { get; set; } = true;

        [Column(Order = 5)]
        [MaxLength(256)]
        public string Remark { get; set; }

        public List<Availability> Availabilities { get; set; }

        
        


        //Constructor
        public Participation()
        {
            Availabilities = new List<Availability>();
        }

        public Participation(Guid id) : base(id)
        {
            Availabilities = new List<Availability>();

        }
    }
}