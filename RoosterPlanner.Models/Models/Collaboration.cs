using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Collaboration : Entity
    {

        [Column(Order = 1)]
        public Guid WantsToWorkWithId { get; set; }
        [ForeignKey("WantsToWorkWithId")]
        public Participation WantsToWorkWith { get; set; }

        [Column(Order = 2)]
        public Guid IsWantedById { get; set; }
        [ForeignKey("IsWantedById")]
        public Participation IsWantedBy { get; set; }
        

        public Collaboration()
        {
        }

        public Collaboration(Guid Id) : base(Id)
        {
        }
    }
}