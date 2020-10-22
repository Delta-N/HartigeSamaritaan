using System;
using System.Collections.Generic;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ProjectDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PictureUri { get; set; }
        public string WebsiteUrl { get; set; }
        public bool Closed { get; set; }
        public List<Task> Tasks { get; set; }

        //Constructor
        public ProjectDetailsViewModel()
        {
            this.Tasks = new List<Task>();
        }
        public static ProjectDetailsViewModel CreateVm(Project project)
        {
            return new ProjectDetailsViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Address = project.Address,
                City = project.City,
                Description = project.Description,
                PictureUri = project.PictureUri,
                WebsiteUrl = project.WebsiteUrl,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Closed = project.Closed
            };
        }
    }
}
