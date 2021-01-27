using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using RoosterPlanner.Data.Context.Seed;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Types;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class AvailabilityRepositoryTest : BaseUnitTest
    {
        private readonly Mock<AvailabilityRepository> availabilityRepository;

        public AvailabilityRepositoryTest()
        {
            availabilityRepository = new Mock<AvailabilityRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "60a8d986-4588-4e7f-b3d5-4023905397f1")]
        [InlineData("e86bb765-27ab-404f-b140-211505d869fe", "00000000-0000-0000-0000-000000000000")]
        [InlineData("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
        public async Task FindAvailabilitiesAsync_Should_Throw_Exception_When_Ids_EMPTYGUIDS(string projectId,
            string userId)
        {
            //Arrange
            //Act
            List<Availability> availabilities = await availabilityRepository.Object.FindAvailabilitiesAsync(
                Guid.Parse(projectId), Guid.Parse(userId));
            //Assert 
            Assert.Null(availabilities);
        }

        [Fact]
        public async Task FindAvailabilitiesAsync_Should_Include_Related_Data_And_Filter_References()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");
            Guid personId = Guid.Parse("60a8d986-4588-4e7f-b3d5-4023905397f1");
            //Act
            List<Availability> availabilities = await availabilityRepository.Object.FindAvailabilitiesAsync(
                projectId, personId);
            //Assert 
            Assert.NotNull(availabilities);
            Assert.NotEmpty(availabilities);
            foreach (Availability availability in availabilities)
            {
                Assert.NotNull(availability.Participation);
                Assert.NotNull(availability.Shift);
                Assert.NotNull(availability.Shift.Task);
                Assert.NotNull(availability.Shift.Task.Instruction);
                Assert.Equal(projectId, availability.Participation.ProjectId);
                Assert.Equal(personId, availability.Participation.PersonId);
                Assert.Null(availability.Participation.Availabilities);
                Assert.Null(availability.Shift.Availabilities);
                Assert.Null(availability.Shift.Task.Shifts);
            }
        }

        [Fact]
        public async Task GetActiveAvailabilities_Should_Throw_Exception_When_Ids_EMPTYGUIDS()
        {
            //Arrange

            //Act
            List<Availability> availabilities = await availabilityRepository.Object.GetActiveAvailabilities(
                Guid.Empty);
            //Assert 
            Assert.Null(availabilities);
        }

        [Fact]
        public async Task GetActiveAvailabilities_Should_Include_Related_Data_And_Filter_References()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");
            Guid personId = Guid.Parse("60a8d986-4588-4e7f-b3d5-4023905397f1");
            Guid participationId = Helper.ConcatGuid(projectId, personId);
            //Act
            List<Availability> availabilities = await availabilityRepository.Object.GetActiveAvailabilities(
                participationId);
            //Assert 
            Assert.NotNull(availabilities);
            Assert.NotEmpty(availabilities);

            foreach (Availability availability in availabilities)
            {
                Assert.Equal(participationId, availability.ParticipationId);
                Assert.NotNull(availability.Shift);
                Assert.Equal(AvailibilityType.Scheduled, availability.Type);
                Assert.True(availability.Shift.Date >= DateTime.Today);
                Assert.Null(availability.Shift.Availabilities);
            }
        }

        [Fact]
        public async Task GetScheduledAvailabilities_Should_Throw_Exception_When_Ids_EMPTYGUIDS()
        {
            //Arrange

            //Act
            List<Availability> availabilities = await availabilityRepository.Object.GetScheduledAvailabilities(
                Guid.Empty);
            //Assert 
            Assert.Null(availabilities);
        }

        [Fact]
        public async Task GetScheduledAvailabilities_Should_Include_Related_Data_And_Filter_References()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");
            Guid personId = Guid.Parse("60a8d986-4588-4e7f-b3d5-4023905397f1");
            Guid participationId = Helper.ConcatGuid(projectId, personId);
            //Act
            List<Availability> availabilities = await availabilityRepository.Object.GetScheduledAvailabilities(
                participationId);
            //Assert 
            Assert.NotNull(availabilities);
            Assert.NotEmpty(availabilities);
            DateTime current = availabilities[0].Shift.Date;
            foreach (Availability availability in availabilities)
            {
                Assert.NotNull(availability.Participation);
                Assert.NotNull(availability.Participation.Project);
                Assert.NotNull(availability.Shift);
                Assert.NotNull(availability.Shift.Task);
                Assert.NotNull(availability.Shift.Task.Instruction);
                Assert.Equal(participationId, availability.ParticipationId);
                Assert.Equal(AvailibilityType.Scheduled, availability.Type);
                Assert.True(availability.Shift.Date >= current);
                current = availability.Shift.Date;
                Assert.Null(availability.Shift.Task.Shifts);
                Assert.Null(availability.Shift.Availabilities);
                Assert.Null(availability.Participation.Availabilities);
            }
        }

        [Fact]
        public async Task GetScheduledAvailabilities_OVERLOAD_Should_Return_Null_When_Ids_EMPTYGUIDS()
        {
            //Arrange
            DateTime knownDateTime = roosterPlannerContextMock.Object.Shifts.First().Date;
            //Act
            List<Availability> availabilities = await availabilityRepository.Object.GetScheduledAvailabilities(
                Guid.Empty, knownDateTime);
            //Assert 
            Assert.Null(availabilities);
        }

        [Fact]
        public async Task GetScheduledAvailabilities_OVERLOAD_Should_Include_Related_Data_And_Filter_References()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");
            DateTime knownDateTime = roosterPlannerContextMock.Object.Shifts.First().Date;
            //Act
            List<Availability> availabilities = await availabilityRepository.Object.GetScheduledAvailabilities(
                projectId, knownDateTime);
            //Assert 
            Assert.NotNull(availabilities);
            Assert.NotEmpty(availabilities);
            foreach (Availability availability in availabilities)
            {
                Assert.NotNull(availability.Participation);
                Assert.NotNull(availability.Participation.Person);
                Assert.NotNull(availability.Shift);
                Assert.NotNull(availability.Shift.Task);
                Assert.NotNull(availability.Shift.Task.Instruction);
                Assert.Equal(projectId, availability.Participation.ProjectId);
                Assert.Equal(AvailibilityType.Scheduled, availability.Type);
                Assert.Equal(knownDateTime, availability.Shift.Date);
                Assert.Null(availability.Shift.Availabilities);
                Assert.Null(availability.Shift.Task.Shifts);
                Assert.Null(availability.Participation.Person.Participations);
                Assert.Null(availability.Participation.Availabilities);
            }
        }

        [Fact]
        public async Task GetAvailability_Should_Throw_Exception_When_Ids_EMPTYGUIDS()
        {
            //Arrange
            Guid participationId = Guid.Empty;
            Guid shiftId = Guid.Empty;
            //Act
            //Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => availabilityRepository.Object.GetAvailability(participationId, shiftId));
        }

        [Fact]
        public async Task GetAvailability_Should_Return_Correct_Entities()
        {
            //Arrange
            Guid participationId = roosterPlannerContextMock.Object.Participations.First().Id;
            Guid shiftId = roosterPlannerContextMock.Object.Availabilities
                .First(a => a.ParticipationId == participationId)
                .ShiftId;
            //Act
            Availability availability = await availabilityRepository.Object.GetAvailability(participationId, shiftId);

            //Assert
            Assert.NotNull(availability);
            Assert.Equal(participationId, availability.ParticipationId);
            Assert.Equal(shiftId, availability.ShiftId);
        }
    }
}