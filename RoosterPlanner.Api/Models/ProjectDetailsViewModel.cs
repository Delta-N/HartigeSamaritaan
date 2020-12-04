using System;
using System.Collections.Generic;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ProjectDetailsViewModel : EntityViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public DateTime ParticipationStartDate { get; set; }
        public DateTime? ParticipationEndDate { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public string PictureUri { get; set; }
        public string WebsiteUrl { get; set; }
        public bool Closed { get; set; }
        public List<Task> Tasks { get; set; }

        //Constructor
        public ProjectDetailsViewModel()
        {
            Tasks = new List<Task>();
        }
        public static ProjectDetailsViewModel CreateVm(Project project)
        {
            return new()
            {
                Id = project.Id,
                Name = project.Name,
                Address = project.Address,
                City = project.City,
                Description = project.Description,
                PictureUri = project.PictureUri,
                WebsiteUrl = project.WebsiteUrl,
                ParticipationStartDate = project.ParticipationStartDate,
                ParticipationEndDate = project.ParticipationEndDate,
                ProjectStartDate = project.ProjectStartDate,
                ProjectEndDate = project.ProjectEndDate,
                Closed = project.Closed,
                LastEditDate = project.LastEditDate,
                LastEditBy = project.LastEditBy,
                RowVersion = project.RowVersion
            };
        }

        public static Project CreateProject(ProjectDetailsViewModel projectDetailsViewModel)
        {
            return new(projectDetailsViewModel.Id)
            {
                Name = projectDetailsViewModel.Name,
                Address = projectDetailsViewModel.Address,
                City = projectDetailsViewModel.City,
                Description = projectDetailsViewModel.Description,
                PictureUri = projectDetailsViewModel.PictureUri,
                WebsiteUrl = projectDetailsViewModel.WebsiteUrl,
                ParticipationStartDate = projectDetailsViewModel.ParticipationStartDate,
                ParticipationEndDate = projectDetailsViewModel.ParticipationEndDate,
                ProjectStartDate = projectDetailsViewModel.ProjectStartDate,
                ProjectEndDate = projectDetailsViewModel.ProjectEndDate,
                Closed = projectDetailsViewModel.Closed,
                LastEditDate = projectDetailsViewModel.LastEditDate,
                LastEditBy = projectDetailsViewModel.LastEditBy,
                RowVersion = projectDetailsViewModel.RowVersion
            };
        }
    }
}