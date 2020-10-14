using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models.Models
{
    public class Collaboration : Entity
    {

        [Column(Order = 1)]
        public Guid WantsToWorkWithId { get; set; }
        [ForeignKey("WantsToWorkWith")]
        public Participation WantsToWorkWith { get; set; }

        [Column(Order = 2)]
        public Guid IsWantedById { get; set; }
        [ForeignKey("IsWantedBy")]
        public Participation IsWantedBy { get; set; }
        

        public Collaboration()
        {
        }

        public Collaboration(Guid Id) : base(Id)
        {
        }
    }
}