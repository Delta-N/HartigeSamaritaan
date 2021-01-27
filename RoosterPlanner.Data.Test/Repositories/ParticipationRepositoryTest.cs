using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using RoosterPlanner.Data.Context.Seed;
using RoosterPlanner.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class ParticipationRepositoryTest : BaseUnitTest
    {
        private readonly Mock<ParticipationRepository> participationRepositoryMock;

        public ParticipationRepositoryTest()
        {
            participationRepositoryMock = new Mock<ParticipationRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Fact]
        public async Task GetActiveParticipationsAsync_Should_Return_Throw_Exception()
        {
            //Arrange
            //Act
            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => participationRepositoryMock.Object.GetActiveParticipationsAsync(Guid.Empty));
        }

        [Fact]
        public async Task GetActiveParticipationsAsync_Should_Return_Null_When_Person_Not_In_DB()
        {
            //Arrange
            //Act
            List<Participation> participations =
                await participationRepositoryMock.Object.GetActiveParticipationsAsync(Guid.NewGuid());
            //Assert
            Assert.Empty(participations);
        }

        [Fact]
        public async Task GetActiveParticipationsAsync_Should_Include_Project_And_Person()
        {
            //Arrange
            Project project = ProjectSeed.Seed().First();
            Person person = PersonSeed.Seed().First();
            Participation participation = new Participation()
            {
                ProjectId = project.Id,
                PersonId = person.Id,
            };
            participationRepositoryMock.Object.Add(participation);
            roosterPlannerContextMock.Object.SaveChanges();

            //Act
            List<Participation> participations =
                await participationRepositoryMock.Object.GetActiveParticipationsAsync(person.Id);
            //Assert
            foreach (Participation participation1 in participations)
            {
                Assert.NotNull(participation1.Project);
                Assert.NotNull(participation1.Person);
            }
        }

        [Fact]
        public async Task GetActiveParticipationsAsync_Should_Filter_Closed_Projects()
        {
            //Arrange
            await foreach (Project project in roosterPlannerContextMock.Object.Projects)
            {
                project.Closed = true;
                roosterPlannerContextMock.Object.Projects.Update(project);
            }

            Person person = PersonSeed.Seed().First();
            
            roosterPlannerContextMock.Object.SaveChanges();

            //Act
            List<Participation> participations =
                await participationRepositoryMock.Object.GetActiveParticipationsAsync(person.Id);
            //Assert
            Assert.Empty(participations);
        }

        [Fact]
        public async Task GetActiveParticipationsAsync_Should_Filter_Inactive_Participations()
        {
            //Arrange
   
            Person person = PersonSeed.Seed().First();
            await foreach (Participation participation in roosterPlannerContextMock.Object.Participations)
            {
                participation.Active = false;
                roosterPlannerContextMock.Object.Participations.Update(participation);
            }

            roosterPlannerContextMock.Object.SaveChanges();

            //Act
            List<Participation> participations =
                await participationRepositoryMock.Object.GetActiveParticipationsAsync(person.Id);
            //Assert
            Assert.Empty(participations);
        }

        [Fact]
        public async Task GetActiveParticipationsAsync_Should_Filter_ParticipationEndDate_In_The_Past()
        {
            //Arrange
            Project project = roosterPlannerContextMock.Object.Projects.First();
            project.ParticipationEndDate = DateTime.Today.AddDays(-7);
            Person person = PersonSeed.Seed().First();

            roosterPlannerContextMock.Object.Projects.Update(project);
            roosterPlannerContextMock.Object.SaveChanges();

            //Act
            List<Participation> participations =
                await participationRepositoryMock.Object.GetActiveParticipationsAsync(person.Id);
            //Assert
            foreach (Participation participation in participations)
            {
                Assert.NotEqual(project.Id, participation.ProjectId);
            }
        }

        [Fact]
        public async Task GetActiveParticipationsAsync_Should_Include_ParticipationEndDate_Is_Null()
        {
            //Arrange
            Project project = ProjectSeed.Seed().First();
            project.ParticipationEndDate = null;
            Person person = PersonSeed.Seed().First();
            roosterPlannerContextMock.Object.SaveChanges();

            //Act
            List<Participation> participations =
                await participationRepositoryMock.Object.GetActiveParticipationsAsync(person.Id);
            //Assert
            Assert.NotEmpty(participations);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "48663dfa-4711-4c77-a036-4bf400d4d524")]
        [InlineData("48663dfa-4711-4c77-a036-4bf400d4d524", "00000000-0000-0000-0000-000000000000")]
        [InlineData("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
        public async Task GetSpecificParticipationAsync_Should_Return_Null_When_Parameters_Null(string personId,
            string projectId)
        {
            //Arrange
            Guid person = Guid.Parse(personId);
            Guid project = Guid.Parse(projectId);
            //Act

            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => participationRepositoryMock.Object.GetSpecificParticipationAsync(person, project));
        }

        [Fact]
        public async Task GetSpecificParticipationAsync_Should_Return_Null_When_IDS_Not_In_DB()
        {
            //Arrange
            Guid person = Guid.NewGuid();
            Guid project = Guid.NewGuid();
            //Act
            Participation participation =
                await participationRepositoryMock.Object.GetSpecificParticipationAsync(person, project);
            //Assert
            Assert.Null(participation);
        }

        [Fact]
        public async Task GetSpecificParticipationAsync_Should_Include_Related_Data()
        {
            //Arrange
            Guid person = PersonSeed.Seed().First().Id;
            Guid projectId = ProjectSeed.Seed().First().Id;


            //Act
            Participation locatedParticipation =
                await participationRepositoryMock.Object.GetSpecificParticipationAsync(person, projectId);
            //Assert
            Assert.NotNull(locatedParticipation);
            Assert.Equal(person, locatedParticipation.PersonId);
            Assert.Equal(projectId, locatedParticipation.ProjectId);
            Assert.NotNull(locatedParticipation.Person);
            Assert.NotNull(locatedParticipation.Project);
            Assert.NotNull(locatedParticipation.Project.PictureUri);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public async Task GetParticipations_Should_Return_Null_When_ID_Is_Null(string id)
        {
            //Arrange
            Guid projectId = Guid.Parse(id);

            //Act

            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => participationRepositoryMock.Object.GetParticipations(projectId));
        }

        [Fact]
        public async Task GetParticipations_Should_Include_Project()
        {
            //Arrange


            //Act
            List<Participation> participations = await participationRepositoryMock.Object.GetParticipations(
                Guid.Parse("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"));

            //Assert
            Assert.Equal(PersonSeed.Seed().Count, participations.Count);
            foreach (Participation participation in participations)
            {
                Assert.NotNull(participation.Project);
            }
        }

        [Fact]
        public async Task GetParticipations_Should_Return_Null_When_Project_Not_In_DB()
        {
            //Arrange
            Project project = new Project(Guid.NewGuid());
            //Act
            List<Participation> participations = await participationRepositoryMock.Object.GetParticipations(project.Id);
            //Assert
            Assert.Empty(participations);
        }

        [Fact]
        public async Task GetParticipationsWithAvailabilities_Should_Return_Null_When_ID_Is_Null()
        {
            //Arrange

            //Act

            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => participationRepositoryMock.Object.GetParticipationsWithAvailabilities(Guid.Empty));
        }

        [Fact]
        public async Task GetParticipationsWithAvailabilities_Should_Include_Related_Data()
        {
            //Arrange

            //Act
            List<Participation> participations = await participationRepositoryMock.Object.GetParticipationsWithAvailabilities(
                Guid.Parse("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"));
            //Assert
            foreach (Participation participation in participations)
            {
                Assert.Equal(Guid.Parse("55c92c6a-067b-442a-b33d-b8ce35cf1d8a"),participation.ProjectId);
                Assert.NotNull(participation.Project);
                Assert.NotNull(participation.Availabilities);
                Assert.True(participation.Availabilities.Count>0);
                foreach (Availability availability in participation.Availabilities)
                {
                    Assert.NotNull(availability.Shift);
                    Assert.NotNull(availability.Shift.Task);
                }
            }
        }

        [Fact]
        public async Task GetParticipationsWithAvailabilities_Should_Return_Null_When_Project_Not_In_DB()
        {
            //Arrange

            //Act
            List<Participation> participations =
                await participationRepositoryMock.Object.GetParticipationsWithAvailabilities(
                    Guid.NewGuid());
            //Assert
            Assert.Empty(participations);
        }
    }
}