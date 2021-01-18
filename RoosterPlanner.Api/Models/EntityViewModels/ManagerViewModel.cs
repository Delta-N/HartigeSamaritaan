using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ManagerViewModel : EntityViewModel
    {
        public Guid ProjectId { get; set; }
        public ProjectDetailsViewModel Project { get; set; }
        public Guid PersonId { get; set; }
        public PersonViewModel Person { get; set; }

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