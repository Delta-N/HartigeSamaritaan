using System;
using RoosterPlanner.Models.Models;
namespace RoosterPlanner.Api.Models.EntityViewModels
{
    public class ProjectTaskViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the TaskId 
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Creates a ViewModel from a ProjectTask.
        /// </summary>
        /// <param name="projectTask"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a ProjectTask from a ViewModel.
        /// </summary>
        /// <param name="projectTaskViewModel"></param>
        /// <returns></returns>
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