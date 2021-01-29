using System;
using Moq;
using RoosterPlanner.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class TaskRepositoryTest : BaseUnitTest
    {
        private readonly Mock<TaskRepository> taskRepositoryMock;

        public TaskRepositoryTest()
        {
            taskRepositoryMock = new Mock<TaskRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Fact]
        public async Task GetTaskAsync_Should_Return_Exception_When_Id_Is_Null()
        {
            //Arrange

            //Act
            Models.Task t  =await taskRepositoryMock.Object.GetTaskAsync(Guid.Empty);

            //Assert
            Assert.Null(t);
        }

        [Fact]
        public async Task GetTaskAsync_Should_Include_Related_Data()
        {
            //Arrange
            
            //Act
            Models.Task t =
                await taskRepositoryMock.Object.GetTaskAsync(Guid.Parse("1316726a-a651-4cef-800d-a74746ca3d74"));
            //Assert
            Assert.NotNull(t);
            Assert.Equal(Guid.Parse("1316726a-a651-4cef-800d-a74746ca3d74"),t.Id);
            Assert.NotNull(t.Instruction);
            Assert.NotNull(t.Category);
            Assert.NotNull(t.Requirements);
            Assert.NotEmpty(t.Requirements);
            foreach (Requirement tRequirement in t.Requirements)
            {
                Assert.NotNull(tRequirement.CertificateType);
            }
        }

        [Fact]
        public async Task GetTaskAsync_Should_Filter_Category_Tasks()
        {
            //Arrange

            //Act
            Models.Task t =
                await taskRepositoryMock.Object.GetTaskAsync(Guid.Parse("1316726a-a651-4cef-800d-a74746ca3d74"));
            //Assert
            Assert.Null(t.Category.Tasks);
        }

        [Fact]
        public async Task GetTaskAsync_Should_Filter_Requirement_Task()
        {
            //Arrange

            //Act
            Models.Task t =
                await taskRepositoryMock.Object.GetTaskAsync(Guid.Parse("1316726a-a651-4cef-800d-a74746ca3d74"));
            //Assert
            foreach (Requirement tRequirement in t.Requirements)
            {
                Assert.Null(tRequirement.Task);
            }
        }

        [Fact]
        public async Task SearchTasksAsync_Should_Return_Exception_When_Id_Is_Null()
        {
            //Arrange

            //Act

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => taskRepositoryMock.Object.SearchTasksAsync(null));
        }

        [Fact]
        public void  SearchTasksAsync_Should_Include_Related_Data()
        {
            //Arrange

            //Act

            //Assert
        }

    }
}