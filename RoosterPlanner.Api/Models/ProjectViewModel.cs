using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ProjectViewModel : EntityViewModel
    {
        public string Name { get; set; }
        public DateTime ParticipationStartDate { get; set; }
        public DateTime? ParticipationEndDate { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public bool Closed { get; set; }

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