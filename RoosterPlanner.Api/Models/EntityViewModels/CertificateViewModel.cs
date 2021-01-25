using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class CertificateViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the DateIssued 
        /// </summary>
        public DateTime DateIssued { get; set; }

        /// <summary>
        /// Gets or sets the DateExpired 
        /// </summary>
        public DateTime? DateExpired { get; set; }

        /// <summary>
        /// Gets or sets the Person 
        /// </summary>
        public PersonViewModel Person { get; set; }

        /// <summary>
        /// Gets or sets the CertificateType 
        /// </summary>
        public CertificateTypeViewModel CertificateType { get; set; }

        /// <summary>
        /// Creates a ViewModel from a certifiacte.
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public static CertificateViewModel CreateVm(Certificate certificate)
        {
            if (certificate == null)
                return null;
            CertificateViewModel vm = new CertificateViewModel
            {
                DateIssued = certificate.DateIssued,
                DateExpired = certificate.DateExpired,
                Id = certificate.Id,
                LastEditBy = certificate.LastEditBy,
                LastEditDate = certificate.LastEditDate,
                RowVersion = certificate.RowVersion
            };
            if (certificate.Person != null)
                vm.Person = PersonViewModel.CreateVmFromPerson(certificate.Person);

            if (certificate.CertificateType != null)
                vm.CertificateType = CertificateTypeViewModel.CreateVm(certificate.CertificateType);

            return vm;
        }

        /// <summary>
        /// Create a Certificate from a ViewModel.
        /// </summary>
        /// <param name="certificateViewModel"></param>
        /// <returns></returns>
        public static Certificate CreateCertificate(CertificateViewModel certificateViewModel)
        {
            if (certificateViewModel == null)
                return null;
            Certificate certificate = new Certificate(certificateViewModel.Id)
            {
                DateIssued = certificateViewModel.DateIssued,
                DateExpired = certificateViewModel.DateExpired,
                LastEditBy = certificateViewModel.LastEditBy,
                LastEditDate = certificateViewModel.LastEditDate,
                RowVersion = certificateViewModel.RowVersion
            };
            if (certificateViewModel.Person != null)
            {
                certificate.Person = PersonViewModel.CreatePerson(certificateViewModel.Person);
                certificate.PersonId = certificate.Person.Id;
            }

            if (certificateViewModel.CertificateType == null) return certificate;

            certificate.CertificateType =
                CertificateTypeViewModel.CreateCertificateType(certificateViewModel.CertificateType);
            certificate.CertificateTypeId = certificate.CertificateType.Id;

            return certificate;
        }
    }
}