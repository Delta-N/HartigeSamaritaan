using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class RequirementViewModel : EntityViewModel
    {
        public CertificateTypeViewModel CertificateType { get; set; }
        public TaskViewModel Task { get; set; }

        public static RequirementViewModel CreateVm(Requirement requirement)
        {
            if (requirement == null)
                return null;

            RequirementViewModel vm = new RequirementViewModel
            {
                Id = requirement.Id,
                LastEditDate = requirement.LastEditDate,
                LastEditBy = requirement.LastEditBy,
                RowVersion = requirement.RowVersion
            };
            if (requirement.CertificateType != null)
                vm.CertificateType = CertificateTypeViewModel.CreateVm(requirement.CertificateType);
            if (requirement.Task != null)
                vm.Task = TaskViewModel.CreateVm(requirement.Task);

            return vm;
        }

        public static Requirement CreateRequirement(RequirementViewModel vm)
        {
            if (vm == null)
                return null;

            Requirement requirement = new Requirement(vm.Id)
            {
                LastEditDate = vm.LastEditDate,
                LastEditBy = vm.LastEditBy,
                RowVersion = vm.RowVersion
            };
            if (vm.CertificateType != null)
            {
                requirement.CertificateType = CertificateTypeViewModel.CreateCertificateType(vm.CertificateType);
                requirement.CertificateTypeId = requirement.CertificateType.Id;
                
            }

            if (vm.Task != null)
            {
                requirement.Task = TaskViewModel.CreateTask(vm.Task);
                requirement.TaskId = requirement.Task.Id;
            }

            return requirement;
        }
    }
}