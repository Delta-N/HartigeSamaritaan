using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Document : Entity
    {
        [Column(Order = 1)]
        [Required, MaxLength(64)]
        public string Name { get; set; }

        [Column(Order = 2)] 
        [Required,MaxLength(256)] 
        public string DocumentUri { get; set; }

        public List<Person> ProfilePictures { get; set; }
        public List<Project> ProjectPictures { get; set; }
        public List<Task> Instructions { get; set; }

        public Document(Guid id) : base(id)
        {
            ProfilePictures = new List<Person>();
            ProjectPictures = new List<Project>();
            Instructions = new List<Task>();
        }

        public Document()
        {
            ProfilePictures = new List<Person>();
            ProjectPictures = new List<Project>();
            Instructions = new List<Task>();
        }
    }
}