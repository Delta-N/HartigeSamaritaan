using System;
using System.Linq;
using Moq;
using RoosterPlanner.Models.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class CertificateRepositoryTest : BaseUnitTest
    {
        private readonly Mock<CertificateRepository> certificateRepository;

        public CertificateRepositoryTest()
        {
            certificateRepository = new Mock<CertificateRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Fact]
        public async Task GetCertificateAsync_Should_Return_Exeption_When_EMPTYGUID()
        {
            //Arrange
            //Act
            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => certificateRepository.Object.GetCertificateAsync(Guid.Empty));
        }

        [Fact]
        public async Task GetCertificateAsync_Should_Include_References()
        {
            //Arrange
            //Act
            Certificate certificate =
                await certificateRepository.Object.GetCertificateAsync(roosterPlannerContextMock.Object.Certificates
                    .First().Id);
            //Assert
            Assert.NotNull(certificate);
            Assert.NotNull(certificate.CertificateType);
            Assert.NotNull(certificate.Person);
        }

        [Fact]
        public async Task GetCertificateAsync_Should_Return_CorrectEntity()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Certificate certificate = new Certificate(id);
            certificate.CertificateTypeId = Guid.Parse("337f1542-159b-4c88-8335-506e34a65754"); //HACCP
            certificate.PersonId = Guid.Parse("60a8d986-4588-4e7f-b3d5-4023905397f1            "); //Stuart Ridley
            roosterPlannerContextMock.Object.Certificates.Add(certificate);
            roosterPlannerContextMock.Object.SaveChanges();
            //Act

            Certificate addedCertificate = await certificateRepository.Object.GetCertificateAsync(id);
            //Assert
            Assert.NotNull(addedCertificate);
            Assert.Equal(id,addedCertificate.Id);
            Assert.NotNull(addedCertificate.CertificateType);
            Assert.NotNull(addedCertificate.Person);
        }
    }
}