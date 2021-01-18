using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class CertificateTypeViewModel : EntityViewModel
    {
        public string Name { get; set; }
        public string Level { get; set; }

        public static CertificateTypeViewModel CreateVm(CertificateType certificateType)
        {
            if (certificateType == null)
                return null;
            return new CertificateTypeViewModel
            {
                Name = certificateType.Name,
                Level = certificateType.Level,
                Id=certificateType.Id,
                LastEditBy = certificateType.LastEditBy,
                LastEditDate = certificateType.LastEditDate,
                RowVersion = certificateType.RowVersion

            };
        }

        public static CertificateType CreateCertificateType(CertificateTypeViewModel certificateTypeViewModel)
        {
            if (certificateTypeViewModel == null)
                return null;
            return new CertificateType(certificateTypeViewModel.Id)
            {
                Name = certificateTypeViewModel.Name,
                Level = certificateTypeViewModel.Level,
                LastEditBy = certificateTypeViewModel.LastEditBy,
                LastEditDate = certificateTypeViewModel.LastEditDate,
                RowVersion = certificateTypeViewModel.RowVersion
            };
        }
    }
}