using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Document : Entity
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        [Column(Order = 1)]
        [Required, MaxLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the DocumentUri
        /// </summary>
        [Column(Order = 2)] 
        [Required,MaxLength(256)] 
        public string DocumentUri { get; set; }

        /// <summary>
        /// Gets or sets the ProfilePictures
        /// </summary>
        public List<Person> ProfilePictures { get; set; }

        /// <summary>
        /// Gets or sets the ProjectPictures
        /// </summary>
        public List<Project> ProjectPictures { get; set; }

        /// <summary>
        /// Gets or sets the Instructions
        /// </summary>
        public List<Task> Instructions { get; set; }
        //constructor
        public Document(Guid id) : base(id)
        {
            ProfilePictures = new List<Person>();
            ProjectPictures = new List<Project>();
            Instructions = new List<Task>();
        }
        //constructor
        public Document()
        {
            ProfilePictures = new List<Person>();
            ProjectPictures = new List<Project>();
            Instructions = new List<Task>();
        }
    }
}