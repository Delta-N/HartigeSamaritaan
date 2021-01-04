using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class CertificateViewModel : EntityViewModel
    {
        public DateTime DateIssued { get; set; }

        public DateTime? DateExpired { get; set; }

        public PersonViewModel Person { get; set; }
        public CertificateTypeViewModel CertificateType { get; set; }

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