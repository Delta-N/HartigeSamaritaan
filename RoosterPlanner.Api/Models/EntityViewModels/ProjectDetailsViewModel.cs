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
        public DocumentViewModel PictureUri { get; set; }
        public string WebsiteUrl { get; set; }
        public bool Closed { get; set; }
        public List<Task> Tasks { get; set; }
        public string ContactAdres { get; set; }

        //Constructor
        public ProjectDetailsViewModel()
        {
            Tasks = new List<Task>();
        }
        public static ProjectDetailsViewModel CreateVm(Project project)
        {
            if (project == null)
                return null;
            ProjectDetailsViewModel vm = new ProjectDetailsViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Address = project.Address,
                City = project.City,
                Description = project.Description,
                ParticipationStartDate = project.ParticipationStartDate,
                WebsiteUrl = project.WebsiteUrl,
                ParticipationEndDate = project.ParticipationEndDate,
                ProjectStartDate = project.ProjectStartDate,
                ProjectEndDate = project.ProjectEndDate,
                Closed = project.Closed,
                ContactAdres = project.ContactAdres,
                LastEditDate = project.LastEditDate,
                LastEditBy = project.LastEditBy,
                RowVersion = project.RowVersion
            };
            if (project.PictureUri != null)
                vm.PictureUri = DocumentViewModel.CreateVm(project.PictureUri);
            return vm;
        }

        public static Project CreateProject(ProjectDetailsViewModel projectDetailsViewModel)
        {
            if (projectDetailsViewModel == null)
                return null;
            Project project = new Project(projectDetailsViewModel.Id)
            {
                Name = projectDetailsViewModel.Name,
                Address = projectDetailsViewModel.Address,
                City = projectDetailsViewModel.City,
                Description = projectDetailsViewModel.Description,
                WebsiteUrl = projectDetailsViewModel.WebsiteUrl,
                ParticipationStartDate = projectDetailsViewModel.ParticipationStartDate,
                ParticipationEndDate = projectDetailsViewModel.ParticipationEndDate,
                ProjectStartDate = projectDetailsViewModel.ProjectStartDate,
                ProjectEndDate = projectDetailsViewModel.ProjectEndDate,
                Closed = projectDetailsViewModel.Closed,
                ContactAdres = projectDetailsViewModel.ContactAdres,
                LastEditDate = projectDetailsViewModel.LastEditDate,
                LastEditBy = projectDetailsViewModel.LastEditBy,
                RowVersion = projectDetailsViewModel.RowVersion
            };
            if (projectDetailsViewModel.PictureUri == null) return project;
            project.PictureUri = DocumentViewModel.CreateDocument(projectDetailsViewModel.PictureUri);
            project.PictureUriId = project.PictureUri.Id;
            return project;
        }
    }
}