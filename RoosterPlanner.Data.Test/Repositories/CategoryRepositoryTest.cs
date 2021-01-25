using System.Collections.Generic;
using Moq;
using RoosterPlanner.Data.Context.Seed;
using RoosterPlanner.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class CategoryTest : BaseUnitTest
    {
        private readonly Mock<CategoryRepository> categoryRepositoryMock;

        public CategoryTest()
        {
            categoryRepositoryMock = new Mock<CategoryRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Fact]
        public async Task GetAllCategoriesAsync_Should_Return_All_Categories()
        {
            //Arrange

            //Act
            List<Category> categories = await categoryRepositoryMock.Object.GetAllCategoriesAsync();

            //Assert
            Assert.NotNull(categories);
            Assert.NotEmpty(categories);
            Assert.Equal(CategorySeed.Seed().Count,categories.Count);
            
        }
    }
}