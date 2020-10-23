using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ProjectViewModel 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Closed { get; set; }

        public static ProjectViewModel CreateVm(Project project)
        {
            return new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Closed = project.Closed
            };
        }
    }
}