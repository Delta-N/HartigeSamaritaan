using Moq;
using RoosterPlanner.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class DocumentRepositoryTest : BaseUnitTest
    {
        private readonly Mock<DocumentRepository> documentRepositoryMock;

        public DocumentRepositoryTest()
        {
            documentRepositoryMock = new Mock<DocumentRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Fact]
        public async Task GetPPAsync_Should_Return_PP_Document()
        {
            //Arrange

            //Act
            Document document = await documentRepositoryMock.Object.GetPPAsync();

            //Assert
            Assert.NotNull(document);
            Assert.Equal("Privacy Policy",document.Name);
        }
    }
}