using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using RoosterPlanner.Data.Context.Seed;
using RoosterPlanner.Models.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class ManagerRepositoryTest : BaseUnitTest
    {
        private readonly Mock<ManagerRepository> managerRepositoryMock;

        public ManagerRepositoryTest()
        {
            managerRepositoryMock = new Mock<ManagerRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Theory]
        [InlineData("e86bb765-27ab-404f-b140-211505d869fe", "00000000-0000-0000-0000-000000000000")]
        [InlineData("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
        public async Task GetManagerAsync_Should_Throw_Exception_When_Ids_EMPTYGUIDS(string projectId,
            string userId)
        {
            //Arrange
            //Act
            //Assert 
            await Assert.ThrowsAsync<NullReferenceException>(
                () => managerRepositoryMock.Object.GetManagerAsync(Guid.Parse(projectId), Guid.Parse(userId)));
        }

        [Fact]
        public async Task GetManagerAsync_Should_Include_Related_Data()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");
            Guid userId = roosterPlannerContextMock.Object.Managers.First(m => m.ProjectId == projectId).PersonId;

            //Act
            Manager manager = await managerRepositoryMock.Object.GetManagerAsync(projectId, userId);
            //Assert 
            Assert.NotNull(manager);
            Assert.NotNull(manager.Project);
            Assert.NotNull(manager.Person);
            Assert.Equal(projectId, manager.ProjectId);
            Assert.Equal(userId, manager.PersonId);
        }

        [Fact]
        public async Task UserManagesOtherProjectsAsync_Should_Throw_Exception_When_Id_EMPTYGUID()
        {
            //Arrange
            //Act
            //Assert 
            await Assert.ThrowsAsync<NullReferenceException>(
                () => managerRepositoryMock.Object.UserManagesOtherProjectsAsync(Guid.Empty));
        }

        [Fact]
        public async Task UserManagesOtherProjectsAsync_Should_Return_Correct_Entities()
        {
            //Arrange
            Guid userId = roosterPlannerContextMock.Object.Managers.First().PersonId;
            //Act
            List<Manager> managers = await managerRepositoryMock.Object.UserManagesOtherProjectsAsync(userId);
            //Assert 
            Assert.NotNull(managers);
            Assert.NotEmpty(managers);
        }

        [Fact]
        public async Task GetProjectManagersAsync_Should_Throw_Exception_When_Id_EMPTYGUIDS()
        {
            //Arrange
            //Act
            //Assert 
            await Assert.ThrowsAsync<NullReferenceException>(
                () => managerRepositoryMock.Object.GetProjectManagersAsync(Guid.Empty));
        }

        [Fact]
        public async Task GetProjectManagersAsync_Should_Include_Related_Data()
        {
            //Arrange
            Guid projectId = ProjectSeed.Seed().First().Id;
            //Act
            List<Manager> managers = await managerRepositoryMock.Object.GetProjectManagersAsync(projectId);
            //Assert 
            Assert.NotNull(managers);
            Assert.NotEmpty(managers);
            foreach (Manager manager in managers)
            {
                Assert.NotNull(manager.Person);
                Assert.NotNull(manager.Project);
                Assert.Equal(projectId, manager.ProjectId);
            }
        }

        [Fact]
        public async Task GetProjectsManagedByAsync_Should_Throw_Exception_When_Ids_EMPTYGUIDS()
        {
            //Arrange
            //Act
            //Assert 
            await Assert.ThrowsAsync<NullReferenceException>(
                () => managerRepositoryMock.Object.GetProjectsManagedByAsync(Guid.Empty));
        }

        [Fact]
        public async Task GetProjectsManagedByAsync_Should_Include_Related_Data()
        {
            //Arrange
            Guid userId = roosterPlannerContextMock.Object.Managers.First().PersonId;
            //Act
            List<Manager> managers = await managerRepositoryMock.Object.GetProjectsManagedByAsync(userId);
            //Assert 
            Assert.NotNull(managers);
            Assert.NotEmpty(managers);
            foreach (Manager manager in managers)
            {
                Assert.NotNull(manager.Person);
                Assert.NotNull(manager.Project);
                Assert.Equal(userId, manager.PersonId);
                Assert.True(manager.Project.ProjectEndDate >= DateTime.Today);
                Assert.True(!manager.Project.Closed);
            }
        }
    }
}