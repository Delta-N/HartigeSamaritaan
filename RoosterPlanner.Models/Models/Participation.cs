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

        public List<Availability> Availabilities { get; set; }

        public List<Collaboration> WantsToWorkWith { get; set; }
        public List<Collaboration> IsWantedBy { get; set; }
        
        


        //Constructor
        public Participation() : base()
        {
            Availabilities = new List<Availability>();
            WantsToWorkWith=new List<Collaboration>();
            IsWantedBy=new List<Collaboration>();

        }

        public Participation(Guid id) : base(id)
        {
            Availabilities = new List<Availability>();
            WantsToWorkWith=new List<Collaboration>();
            IsWantedBy=new List<Collaboration>();

        }
    }
}