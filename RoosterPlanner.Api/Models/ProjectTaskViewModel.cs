using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ProjectTaskViewModel
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public Guid TaskId { get; set; }

        public static ProjectTaskViewModel CreateVm(ProjectTask projectTask)
        {
            if (projectTask != null)
            {
                ProjectTaskViewModel vm = new ProjectTaskViewModel()
                {
                    Id = projectTask.Id,
                    ProjectId = projectTask.ProjectId,
                };
                if (projectTask.TaskId != null)
                {
                    vm.TaskId = (Guid) projectTask.TaskId;
                }

                return vm;
            }

            return null;
        }

        public static ProjectTask CreateProjectTask(ProjectTaskViewModel projectTaskViewModel)
        {
            if (projectTaskViewModel != null)
            {
                return new ProjectTask(projectTaskViewModel.Id)
                {
                    ProjectId = projectTaskViewModel.ProjectId,
                    TaskId = projectTaskViewModel.TaskId
                };
            }

            return null;
        }
    }
}