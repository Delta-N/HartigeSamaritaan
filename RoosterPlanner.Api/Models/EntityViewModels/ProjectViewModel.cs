using System;
using RoosterPlanner.Models.Models;
namespace RoosterPlanner.Api.Models.EntityViewModels
{
    public class ProjectViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

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
        /// Gets or sets Closed 
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// Creates a ViewModel from a Project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static ProjectViewModel CreateVm(Project project)
        {
            return new()
            {
                Id = project.Id,
                Name = project.Name,
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
    }
}