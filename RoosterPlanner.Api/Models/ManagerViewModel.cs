using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ManagerViewModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectDetailsViewModel Project { get; set; }
        public Guid PersonId { get; set; }
        public PersonViewModel Person { get; set; }

        public static ManagerViewModel CreateVm(Manager manager)
        {
            if (manager == null)
                return null;

            ManagerViewModel vm = new ManagerViewModel()
            {
                Id = manager.Id,
                ProjectId = manager.ProjectId,
                PersonId = manager.PersonId
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
                PersonId = managerViewModel.PersonId
            };
            if (managerViewModel.Project != null)
                manager.Project = ProjectDetailsViewModel.CreateProject(managerViewModel.Project);
            if (managerViewModel.Person != null)
                manager.Person = PersonViewModel.CreatePerson(managerViewModel.Person);
            return manager;
        }
    }
}