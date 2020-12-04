using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ProjectTaskViewModel : EntityViewModel
    {
        public Guid ProjectId { get; set; }
        public Guid TaskId { get; set; }

        public static ProjectTaskViewModel CreateVm(ProjectTask projectTask)
        {
            if (projectTask == null)
                return null;

            ProjectTaskViewModel vm = new ProjectTaskViewModel
            {
                Id = projectTask.Id,
                ProjectId = projectTask.ProjectId,
                LastEditDate = projectTask.LastEditDate,
                LastEditBy = projectTask.LastEditBy,
                RowVersion = projectTask.RowVersion
            };
            if (projectTask.TaskId != Guid.Empty)
            {
                vm.TaskId = projectTask.TaskId;
            }

            return vm;
        }

        public static ProjectTask CreateProjectTask(ProjectTaskViewModel projectTaskViewModel)
        {
            if (projectTaskViewModel != null)
            {
                return new ProjectTask(projectTaskViewModel.Id)
                {
                    ProjectId = projectTaskViewModel.ProjectId,
                    TaskId = projectTaskViewModel.TaskId,
                    LastEditDate = projectTaskViewModel.LastEditDate,
                    LastEditBy = projectTaskViewModel.LastEditBy,
                    RowVersion = projectTaskViewModel.RowVersion
                };
            }

            return null;
        }
    }
}