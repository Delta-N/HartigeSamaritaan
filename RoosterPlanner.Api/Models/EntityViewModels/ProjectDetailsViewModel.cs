using System;
using System.Collections.Generic;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ProjectDetailsViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Address 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the Description 
        /// </summary>
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
        /// Gets or sets the PictureUri 
        /// </summary>
        public DocumentViewModel PictureUri { get; set; }

        /// <summary>
        /// Gets or sets the WebsiteUrl 
        /// </summary>
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// Gets or sets Closed 
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// Gets or sets the Tasks 
        /// </summary>
        public List<Task> Tasks { get; set; }

        /// <summary>
        /// Gets or sets the ContactAdres 
        /// </summary>
        public string ContactAdres { get; set; }

        //Constructor
        public ProjectDetailsViewModel()
        {
            Tasks = new List<Task>();
        }

        /// <summary>
        /// Create a ViewModel from a Project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a Project from a ViewModel.
        /// </summary>
        /// <param name="projectDetailsViewModel"></param>
        /// <returns></returns>
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