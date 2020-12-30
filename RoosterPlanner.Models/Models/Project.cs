using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Project : Entity
    {
        [Column(Order = 1)]
        [Required, MaxLength(64)]
        public string Name { get; set; }

        [Column(Order = 2)]
        [MaxLength(64)]
        public string Address { get; set; }

        [Column(Order = 3)]
        [MaxLength(64)]
        public string City { get; set; }

        [Column(Order = 4)]
        [MaxLength(512)]
        public string Description { get; set; }

        [Column(Order = 5)]
        public DateTime ParticipationStartDate { get; set; }

        [Column(Order = 6)]
        public DateTime? ParticipationEndDate { get; set; }
        
        [Column(Order = 7)]
        public DateTime ProjectStartDate { get; set; }

        [Column(Order = 8)]
        public DateTime ProjectEndDate { get; set; }

        [Column(Order = 9)]

        public Guid? PictureUriId { get; set; }

        [ForeignKey("PictureUriId")]
        public Document PictureUri { get; set; }

        [Column(Order = 10)]
        public string WebsiteUrl { get; set; }

        [Column(Order = 11)]
        public bool Closed { get; set; }

        public List<ProjectTask> ProjectTasks { get; set; }

        public List<Participation> Participations { get; set; }
        
        public List<Shift> Shifts { get; set; }

        //Constructor
        public Project() : this(Guid.Empty)
        {
            ProjectTasks = new List<ProjectTask>();
            Participations = new List<Participation>();
            Shifts=new List<Shift>();
        }

        //Constructor
        public Project(Guid id) : base(id)
        {
            ProjectTasks = new List<ProjectTask>();
            Participations = new List<Participation>();
            Shifts=new List<Shift>();
        }
    }
}