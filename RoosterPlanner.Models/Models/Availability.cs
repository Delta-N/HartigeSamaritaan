using System;
using System.ComponentModel.DataAnnotations.Schema;
using RoosterPlanner.Models.Types;

namespace RoosterPlanner.Models
{
    public class Availability : Entity
    {
        /// <summary>
        /// Gets or sets the ParticipationId 
        /// </summary>
        [Column(Order = 1)]
        public Guid? ParticipationId { get; set; }

        /// <summary>
        /// Gets or sets the Participation
        /// </summary>
        [ForeignKey("ParticipationId")]
        public Participation Participation { get; set; }

        /// <summary>
        /// Gets or sets the ShiftId
        /// </summary>
        [Column(Order = 2)]
        public Guid ShiftId { get; set; }

        /// <summary>
        /// Gets or sets the Shift
        /// </summary>
        [ForeignKey("ShiftId")]
        public Shift Shift { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        [Column(Order = 3)]
        public AvailibilityType Type { get; set; }

        /// <summary>
        /// Gets or sets the Preference 
        /// </summary>
        [Column(Order = 4)]
        public bool Preference { get; set; }

        /// <summary>
        /// Gets or sets the PushEmailSend
        /// </summary>
        [Column(Order = 5)] 
        public bool PushEmailSend { get; set; }

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
