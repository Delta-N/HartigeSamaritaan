using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class CertificateTypeViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Level 
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Creates a ViewModel from a Vertificatetype
        /// </summary>
        /// <param name="certificateType"></param>
        /// <returns></returns>
        public static CertificateTypeViewModel CreateVm(CertificateType certificateType)
        {
            if (certificateType == null)
                return null;
            return new CertificateTypeViewModel
            {
                Name = certificateType.Name,
                Level = certificateType.Level,
                Id = certificateType.Id,
                LastEditBy = certificateType.LastEditBy,
                LastEditDate = certificateType.LastEditDate,
                RowVersion = certificateType.RowVersion
            };
        }

        /// <summary>
        /// Creates a CertificateType from a ViewModel
        /// </summary>
        /// <param name="certificateTypeViewModel"></param>
        /// <returns></returns>
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