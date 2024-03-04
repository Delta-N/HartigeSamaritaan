using System;
using Moq;
using RoosterPlanner.Models.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class PersonRepositoryTest : BaseUnitTest
    {
        private readonly Mock<PersonRepository> personRepositoryMock;

        public PersonRepositoryTest()
        {
            personRepositoryMock = new Mock<PersonRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Fact]
        public async Task GetPersonByOidAsync_Empty_GUID_Should_Return_Null()
        {
            //Arrange

            //Act
            Person p = await personRepositoryMock.Object.GetPersonByOidAsync(Guid.Empty);

            //Assert
            Assert.Null(p);
        }

        [Fact]
        public async Task GetPersonByOidAsync_CertificateType_Should_Not_Include_Certificates()
        {
            //Arrange


            //Act
            Person p = await personRepositoryMock.Object.GetPersonByOidAsync(
                Guid.Parse("3675c1c8-9d2f-4eb6-8751-0b5a4bd36e3b"));

            
            //Assert
            foreach (Certificate pCertificate in p.Certificates)
            {
                Assert.Null(pCertificate.CertificateType.Certificates);
            }
        }

        [Fact]
        public async Task GetPersonByOidAsync_Should_Return_Null_When_ID_Not_In_DB()
        {
            //Arrange


            //Act
            Person p = await personRepositoryMock.Object.GetPersonByOidAsync(
                Guid.NewGuid());

            //Assert
            Assert.Null(p);
            
        }
    }
}