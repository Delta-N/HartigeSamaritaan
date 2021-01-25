using System;
using Moq;
using RoosterPlanner.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class RequirementRepositoryTest : BaseUnitTest
    {
        private readonly Mock<RequirementRepository> requirementRepositoryMock;

        public RequirementRepositoryTest()
        {
            requirementRepositoryMock = new Mock<RequirementRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Fact]
        public async Task GetRequirementAsync_Should_Return_Exception_When_Ids_Are_EmptyGUID()
        {
            //Arrange
        
            //Act

            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => requirementRepositoryMock.Object.GetRequirementAsync(Guid.Empty,Guid.Empty));
        }

        [Fact]
        public async Task GetRequirementAsync_Should_Return_Matching_Entity()
        {
            //Arrange

            //Act
            Requirement requirement = await requirementRepositoryMock.Object.GetRequirementAsync(
                Guid.Parse("1316726a-a651-4cef-800d-a74746ca3d74"), Guid.Parse("793403fd-ae5c-4a49-86df-203893db64c6"));

            //Assert
            Assert.NotNull(requirement);
            Assert.Equal(Guid.Parse("793403fd-ae5c-4a49-86df-203893db64c6"),requirement.CertificateTypeId);
            Assert.Equal(Guid.Parse("1316726a-a651-4cef-800d-a74746ca3d74"),requirement.TaskId);
        }

        [Fact]
        public async Task GetRequirementAsync_OVERLOAD_Should_Return_Exception_When_Id_is_EmptyGUID()
        {
            //Arrange

            //Act
            Requirement requirement = await requirementRepositoryMock.Object.GetRequirementAsync(Guid.Empty);
            //Assert
            Assert.Null(requirement);
        }

        [Fact]
        public async Task GetRequirementAsync_OVERLOAD_Should_Include_Related_Entities()
        {
            //Arrange

            //Act
            Requirement requirement = await requirementRepositoryMock.Object.GetRequirementAsync(
                Guid.Parse("7d43c3e5-0eec-42d9-a586-b7caf9984b9b"));

            //Assert
            Assert.NotNull(requirement);
            Assert.Equal(Guid.Parse("7d43c3e5-0eec-42d9-a586-b7caf9984b9b"), requirement.Id);
            Assert.NotNull(requirement.CertificateType);
            Assert.NotNull(requirement.Task);
        }

        [Fact]
        public async Task GetRequirementAsync_OVERLOAD_Should_Filter_Attributes()
        {
            //Arrange

            //Act
            Requirement requirement = await requirementRepositoryMock.Object.GetRequirementAsync(
                Guid.Parse("7d43c3e5-0eec-42d9-a586-b7caf9984b9b"));

            //Assert
            Assert.NotNull(requirement);
            Assert.Equal(Guid.Parse("7d43c3e5-0eec-42d9-a586-b7caf9984b9b"), requirement.Id);
            Assert.NotNull(requirement.CertificateType);
            Assert.NotNull(requirement.Task);
            Assert.Null(requirement.Task.Requirements);
            Assert.Null(requirement.CertificateType.Requirements);
        }
    }
}