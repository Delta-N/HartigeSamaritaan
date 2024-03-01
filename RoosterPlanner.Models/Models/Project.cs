using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RoosterPlanner.Models.Models
{
    public class Project : Entity
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        [Required, MaxLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Address
        /// </summary>
        [MaxLength(64)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the City
        /// </summary>

        [MaxLength(64)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>

        [MaxLength(512)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ParticipationStartDate 
        /// </summary>
        public DateTime ParticipationStartDate { get; set; }

        /// <summary>
        /// Gets or sets the ParticipationEndDate 
        /// </summary>
        public DateTime? ParticipationEndDate { get; set; }

        /// <summary>
        /// Gets or sets the ProjectStartDate
        /// </summary>
        public DateTime ProjectStartDate { get; set; }

        /// <summary>
        /// Gets or sets the ProjectEndDate
        /// </summary>
        public DateTime ProjectEndDate { get; set; }

        /// <summary>
        /// Gets or sets the PictureUriId
        /// </summary>
        public Guid? PictureUriId { get; set; }

        /// <summary>
        /// Gets or sets the PictureUri
        /// </summary>
        [ForeignKey("PictureUriId")]
        public Document PictureUri { get; set; }

        /// <summary>
        /// Gets or sets the WebsiteUrl
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// Gets or sets Closed
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// Gets or sets the ContactAdres
        /// </summary>
        public string ContactAdres { get; set; }

        /// <summary>
        /// Gets or sets the ProjectTasks
        /// </summary>
        public List<ProjectTask> ProjectTasks { get; set; }

        /// <summary>
        /// Gets or sets the Participations
        /// </summary>
        public List<Participation> Participations { get; set; }

        /// <summary>
        /// Gets or sets the Shifts
        /// </summary>
        public List<Shift> Shifts { get; set; }

        //Constructor
        public Project() : this(Guid.Empty)
        {
            ProjectTasks = new List<ProjectTask>();
            Participations = new List<Participation>();
            Shifts = new List<Shift>();
        }

        //Constructor
        public Project(Guid id) : base(id)
        {
            ProjectTasks = new List<ProjectTask>();
            Participations = new List<Participation>();
            Shifts = new List<Shift>();
        }
    }
}
