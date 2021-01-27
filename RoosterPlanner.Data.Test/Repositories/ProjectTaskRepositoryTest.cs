using System;
using System.Collections.Generic;
using Moq;
using RoosterPlanner.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class ProjectTaskRepositoryTest : BaseUnitTest
    {
        private readonly Mock<ProjectTaskRepository> projectTaskRepository;

        public ProjectTaskRepositoryTest()
        {
            projectTaskRepository = new Mock<ProjectTaskRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Fact]
        public async Task GetAllFromProjectAsync_Should_Throw_Exception_When_Id_EMPTYGUID()
        {
            //Arrange
            //Act
            List<ProjectTask> projectTasks = await projectTaskRepository.Object.GetAllFromProjectAsync(Guid.Empty);
            //Assert
            Assert.Null(projectTasks);
        }

        [Fact]
        public async Task GetAllFromProjectAsync_Should_Include_Related_Entitys_And_Filter_References()
        {
            //Arrange
            //Act
            List<ProjectTask> projectTasks = await projectTaskRepository.Object.GetAllFromProjectAsync(
                Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe"));
            //Assert
            Assert.NotNull(projectTasks);
            Assert.Equal(4, projectTasks.Count);
            foreach (ProjectTask projectTask in projectTasks)
            {
                Assert.Equal(Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe"), projectTask.ProjectId);
                Assert.NotNull(projectTask.Project);
                Assert.NotNull(projectTask.Task);
                Assert.NotNull(projectTask.Task.Requirements);
                foreach (Requirement requirement in projectTask.Task.Requirements)
                {
                    Assert.Null(requirement.Task);
                    Assert.Null(requirement.CertificateType);
                }
            }
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "1316726a-a651-4cef-800d-a74746ca3d74")]
        [InlineData("e86bb765-27ab-404f-b140-211505d869fe", "00000000-0000-0000-0000-000000000000")]
        [InlineData("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
        public async Task GetProjectTaskAsync_Should_Throw_Exception_When_Ids_EMPTYGUID(string projectId,
            string taskId)
        {
            //Arrange
            //Act
            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => projectTaskRepository.Object.GetProjectTaskAsync(Guid.Parse(projectId), Guid.Parse(taskId)));
        }

        [Fact]
        public async Task GetProjectTaskAsync_Should_Return_Correct_Entity()
        {
            //Arrange
            //Act
            ProjectTask projectTask = await projectTaskRepository.Object.GetProjectTaskAsync(Guid.Parse(
                "e86bb765-27ab-404f-b140-211505d869fe"), Guid.Parse("1316726a-a651-4cef-800d-a74746ca3d74"));
            //Assert
            Assert.NotNull(projectTask);
            Assert.Equal(Guid.Parse(
                "e86bb765-27ab-404f-b140-211505d869fe"),projectTask.ProjectId);
            Assert.Equal(Guid.Parse(
                "1316726a-a651-4cef-800d-a74746ca3d74"), projectTask.TaskId);
        }
    }
}