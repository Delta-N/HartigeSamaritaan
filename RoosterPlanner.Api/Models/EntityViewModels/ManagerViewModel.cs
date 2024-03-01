using System;
using RoosterPlanner.Models.Models;
namespace RoosterPlanner.Api.Models.EntityViewModels
{
    public class ManagerViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the ProjectId
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the Project 
        /// </summary>
        public ProjectDetailsViewModel Project { get; set; }

        /// <summary>
        /// Gets or sets the PersonId 
        /// </summary>
        public Guid PersonId { get; set; }

        /// <summary>
        /// Gets or sets the Person 
        /// </summary>
        public PersonViewModel Person { get; set; }

        /// <summary>
        /// Create a ViewModel from a Manager.
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static ManagerViewModel CreateVm(Manager manager)
        {
            if (manager == null)
                return null;

            ManagerViewModel vm = new ManagerViewModel
            {
                Id = manager.Id,
                ProjectId = manager.ProjectId,
                PersonId = manager.PersonId,
                LastEditDate = manager.LastEditDate,
                LastEditBy = manager.LastEditBy,
                RowVersion = manager.RowVersion
            };
            if (manager.Person != null)
                vm.Person = PersonViewModel.CreateVmFromPerson(manager.Person);
            if (manager.Project != null)
                vm.Project = ProjectDetailsViewModel.CreateVm(manager.Project);
            return vm;
        }

        /// <summary>
        /// Creates a Manager from a ViewModel.
        /// </summary>
        /// <param name="managerViewModel"></param>
        /// <returns></returns>
        public static Manager CreateManager(ManagerViewModel managerViewModel)
        {
            if (managerViewModel == null)
                return null;
            Manager manager = new Manager(managerViewModel.Id)
            {
                ProjectId = managerViewModel.ProjectId,
                PersonId = managerViewModel.PersonId,
                LastEditDate = managerViewModel.LastEditDate,
                LastEditBy = managerViewModel.LastEditBy,
                RowVersion = managerViewModel.RowVersion
            };

            if (managerViewModel.Project != null)
                manager.Project = ProjectDetailsViewModel.CreateProject(managerViewModel.Project);
            if (managerViewModel.Person != null)
                manager.Person = PersonViewModel.CreatePerson(managerViewModel.Person);
            return manager;
        }
    }
}