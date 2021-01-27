using System.Collections.Generic;
using Moq;
using RoosterPlanner.Data.Context.Seed;
using RoosterPlanner.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class CertificateTypeRepositoryTest : BaseUnitTest
    {
        private readonly Mock<CertificateTypeRepository> certificateTypeRepository;

        public CertificateTypeRepositoryTest()
        {
            certificateTypeRepository = new Mock<CertificateTypeRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Fact]
        public async Task GetAllCategoriesAsync_Should_Return_All_Categories()
        {
            //Arrange
            //Act
            List<CertificateType> certificateTypes = await certificateTypeRepository.Object.GetAllCategoriesAsync();
            //Assert 
            Assert.NotNull(certificateTypes);
            Assert.NotEmpty(certificateTypes);
            Assert.Equal(CertificateTypeSeed.Seed().Count, certificateTypes.Count);
        }
    }
}