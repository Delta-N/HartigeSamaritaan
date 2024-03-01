using System;
using System.ComponentModel.DataAnnotations.Schema;
using RoosterPlanner.Models.Models.Types;
namespace RoosterPlanner.Models.Models
{
    public class Availability : Entity
    {
        /// <summary>
        /// Gets or sets the ParticipationId 
        /// </summary>
        public Guid? ParticipationId { get; set; }

        /// <summary>
        /// Gets or sets the Participation
        /// </summary>
        [ForeignKey("ParticipationId")]
        public Participation Participation { get; set; }

        /// <summary>
        /// Gets or sets the ShiftId
        /// </summary>
        public Guid ShiftId { get; set; }

        /// <summary>
        /// Gets or sets the Shift
        /// </summary>
        [ForeignKey("ShiftId")]
        public Shift Shift { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public AvailibilityType Type { get; set; }

        /// <summary>
        /// Gets or sets the Preference 
        /// </summary>
        public bool Preference { get; set; }

        /// <summary>
        /// Gets or sets the PushEmailSend
        /// </summary>
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
