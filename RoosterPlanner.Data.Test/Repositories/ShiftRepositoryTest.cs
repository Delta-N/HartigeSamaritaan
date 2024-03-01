using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Models.Models.Types;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace RoosterPlanner.Data.Repositories
{
    public class ShiftRepositoryTest : BaseUnitTest
    {
        private readonly Mock<ShiftRepository> shiftRepositoryTestMock;

        public ShiftRepositoryTest()
        {
            shiftRepositoryTestMock = new Mock<ShiftRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        [Fact]
        public async Task GetByProjectWithAvailabilitiesAsync_Should_Throw_Exception_When_Ids_EMPTYGUIDS()
        {
            //Arrange
            //Act
            List<Shift> shifts = await shiftRepositoryTestMock.Object.GetByProjectWithAvailabilitiesAsync(Guid.Empty);
            //Assert 
            Assert.Null(shifts);
        }

        [Fact]
        public async Task GetByProjectWithAvailabilitiesAsync_Should_Include_Related_Data_And_Filter_References()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");
            //Act
            List<Shift> shifts = await shiftRepositoryTestMock.Object.GetByProjectWithAvailabilitiesAsync(projectId);
            //Assert 
            Assert.NotNull(shifts);
            Assert.NotEmpty(shifts);
            foreach (Shift shift in shifts)
            {
                Assert.NotNull(shift.Project);
                Assert.NotNull(shift.Task);
                Assert.NotNull(shift.Task.Instruction);
                Assert.NotNull(shift.Availabilities);
                Assert.NotEmpty(shift.Availabilities);
                Assert.Equal(projectId, shift.ProjectId);
                Assert.True(shift.Date >= DateTime.Today);
                Assert.Null(shift.Task.Shifts);
                foreach (Availability availability in shift.Availabilities)
                {
                    Assert.Null(availability.Shift);
                }
            }
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "60a8d986-4588-4e7f-b3d5-4023905397f1")]
        [InlineData("e86bb765-27ab-404f-b140-211505d869fe", "00000000-0000-0000-0000-000000000000")]
        [InlineData("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000000")]
        public async Task GetByProjectWithAvailabilitiesAsync_OVERLOAD_Should_Throw_Exception_When_Ids_EMPTYGUIDS(
            string projectId, string userId)
        {
            //Arrange
            //Act
            List<Shift> shifts =
                await shiftRepositoryTestMock.Object.GetByProjectWithAvailabilitiesAsync(Guid.Parse(projectId),
                    Guid.Parse(userId));
            //Assert 
            Assert.Null(shifts);
        }

        [Fact]
        public async Task
            GetByProjectWithAvailabilitiesAsync_OVERLOAD_Should_Include_Related_Data_And_Filter_References()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");
            Guid userId = Guid.Parse("60a8d986-4588-4e7f-b3d5-4023905397f1");
            //Act
            List<Shift> shifts =
                await shiftRepositoryTestMock.Object.GetByProjectWithAvailabilitiesAsync(projectId, userId);
            //Assert 
            Assert.NotNull(shifts);
            Assert.NotEmpty(shifts);
            foreach (Shift shift in shifts)
            {
                Assert.NotNull(shift.Project);
                Assert.NotNull(shift.Task);
                Assert.NotNull(shift.Task.Instruction);
                /*if (shift.TaskId == Guid.Parse("1316726a-a651-4cef-800d-a74746ca3d74") ||
                    shift.TaskId == Guid.Parse("7c1046da-c3b1-498e-bd78-08eed4a53a56"))
                {
                    Assert.NotNull(shift.Task.Requirements);
                    Assert.NotEmpty(shift.Task.Requirements);
                    foreach (Requirement requirement in shift.Task.Requirements)
                    {
                        Assert.NotNull(requirement.CertificateType);
                        Assert.NotNull(requirement.CertificateType.Certificates);
                        Assert.NotEmpty(requirement.CertificateType.Certificates);
                        Assert.Null(requirement.Task.Requirements);
                        Assert.Null(requirement.CertificateType.Certificates);
                        Assert.Null(requirement.CertificateType.Requirements);
                    }
                }*/

                Assert.NotNull(shift.Availabilities);
                Assert.NotEmpty(shift.Availabilities);
                foreach (Availability availability in shift.Availabilities)
                {
                    Assert.NotNull(availability.Participation);
                    Assert.Equal(userId, availability.Participation.PersonId);
                    Assert.Null(availability.Participation.Availabilities);
                    Assert.Null(availability.Shift);
                }

                Assert.Equal(projectId, shift.ProjectId);
                Assert.True(shift.Date >= DateTime.Today);
                Assert.Null(shift.Project.Shifts);
            }
        }

        [Fact]
        public async Task GetUniqueDataAsync_Should_Throw_Exception_When_Id_EMPTYGUIDS()
        {
            //Arrange
            Guid projectId = Guid.NewGuid();

            //Act
            ShiftData data = await shiftRepositoryTestMock.Object.GetUniqueDataAsync(projectId);
            //Assert 
            Assert.NotNull(data);
            Assert.Empty(data.Tasks);
            Assert.Empty(data.Dates);
            Assert.Empty(data.StartTimes);
            Assert.Empty(data.EndTimes);
            Assert.Empty(data.ParticipantsRequired);
        }

        [Fact]
        public async Task GetUniqueDataAsync_Should_Include_Related_Data_And_Filter_References()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");

            //Act
            ShiftData data = await shiftRepositoryTestMock.Object.GetUniqueDataAsync(projectId);
            //Assert 
            Assert.NotNull(data);
            Assert.NotEmpty(data.Tasks);
        }

        [Fact]
        public async Task GetUniqueDataAsync_Should_Return_Correct_Entities()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");

            //Act
            ShiftData data = await shiftRepositoryTestMock.Object.GetUniqueDataAsync(projectId);
            //Assert 
            Assert.NotNull(data);
            Assert.NotEmpty(data.Tasks);

            Assert.NotEmpty(data.Dates);
            Assert.NotEmpty(data.StartTimes);
            Assert.NotEmpty(data.EndTimes);
            Assert.NotEmpty(data.ParticipantsRequired);
        }

        [Fact]
        public async Task GetUniqueDataAsync_Should_Return_No_Duplicates()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");

            //Act
            ShiftData data = await shiftRepositoryTestMock.Object.GetUniqueDataAsync(projectId);
            //Assert 
            Assert.NotNull(data);
            Assert.NotEmpty(data.Tasks);

            Assert.NotEmpty(data.Dates);
            Assert.NotEmpty(data.StartTimes);
            Assert.NotEmpty(data.EndTimes);
            Assert.NotEmpty(data.ParticipantsRequired);

            Assert.Equal(data.Dates.Distinct().Count(), data.Dates.Count);
            Assert.Equal(data.Tasks.Distinct().Count(), data.Tasks.Count);
            Assert.Equal(data.StartTimes.Distinct().Count(), data.StartTimes.Count);
            Assert.Equal(data.EndTimes.Distinct().Count(), data.EndTimes.Count);
            Assert.Equal(data.ParticipantsRequired.Distinct().Count(), data.ParticipantsRequired.Count);
        }

        [Fact]
        public async Task ExportDataAsync_Should_Throw_Exception_When_Id_EMPTYGUIDS()
        {
            //Arrange
            //Act
            //Assert 
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => shiftRepositoryTestMock.Object.ExportDataAsync(Guid.Empty));
        }

        [Fact]
        public async Task ExportDataAsync_Should_Include_Related_Data_And_Filter_References()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");
            //Act
            List<Shift> shifts =
                await shiftRepositoryTestMock.Object.ExportDataAsync(projectId);
            //Assert 
            Assert.NotNull(shifts);
            Assert.NotEmpty(shifts);
            foreach (Shift shift in shifts)
            {
                Assert.NotNull(shift.Task);
                Assert.NotNull(shift.Task.Category);
                Assert.NotNull(shift.Availabilities);
                Assert.NotEmpty(shift.Availabilities);
                foreach (Availability shiftAvailability in shift.Availabilities)
                {
                    Assert.True(shiftAvailability.Type == AvailibilityType.Scheduled);
                    Assert.NotNull(shiftAvailability.Participation);
                    Assert.Null(shiftAvailability.Participation.Availabilities);
                    Assert.Null(shiftAvailability.Shift);
                }

                Assert.Equal(projectId, shift.ProjectId);
                Assert.Null(shift.Task.Shifts);
            }
        }

        [Fact]
        public async Task SearchProjectsAsync_Should_Throw_Exception_When_Filter_Null()
        {
            //Arrange
            //Act
            //Assert 
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => shiftRepositoryTestMock.Object.SearchProjectsAsync(null));
        }

        [Fact]
        public async Task AddShiftsAsync_Should_Return_Null_When_Input_Null()
        {
            //Arrange
            //Act
            List<Shift> shifts = await shiftRepositoryTestMock.Object.AddShiftsAsync(null);
            //Assert 
            Assert.Null(shifts);
        }

        [Fact]
        public async Task AddShiftsAsync_Should_Add_All_Shifts()
        {
            //Arrange
            Guid projectId = Guid.Parse("e86bb765-27ab-404f-b140-211505d869fe");
            int numberOfShifts = roosterPlannerContextMock.Object.Shifts.Count();
            Shift one = new Shift(Guid.NewGuid())
            {
                ProjectId = projectId,
                TaskId = Guid.Parse("7c1046da-c3b1-498e-bd78-08eed4a53a56"),
                StartTime = new TimeSpan(12, 0, 0),
                EndTime = new TimeSpan(15, 0, 0),
                ParticipantsRequired = 1,
                Date = DateTime.Today
            };
            Shift two = new Shift(Guid.NewGuid())
            {
                ProjectId = projectId,
                TaskId = Guid.Parse("7c1046da-c3b1-498e-bd78-08eed4a53a56"),
                StartTime = new TimeSpan(13, 0, 0),
                EndTime = new TimeSpan(16, 0, 0),
                ParticipantsRequired = 2,
                Date = DateTime.Today.AddDays(1)
            };
            List<Shift> shifts = new List<Shift> {one, two};

            //Act
            List<Shift> result = await shiftRepositoryTestMock.Object.AddShiftsAsync(shifts);
            roosterPlannerContextMock.Object.SaveChanges();

            //Assert 
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(numberOfShifts + 2, roosterPlannerContextMock.Object.Shifts.Count());
        }

        [Fact]
        public async Task GetShiftAsync_Should_Return_Null_When_Id_EMPTYGUIDS()
        {
            //Arrange

            //Act
            Shift shift = await shiftRepositoryTestMock.Object.GetShiftAsync(Guid.Empty);
            //Assert 
            Assert.Null(shift);
        }

        [Fact]
        public async Task GetShiftAsync_Should_Include_Related_Data()
        {
            //Arrange
            Guid shiftId = roosterPlannerContextMock.Object.Shifts.First().Id;
            //Act
            Shift shift = await shiftRepositoryTestMock.Object.GetShiftAsync(shiftId);

            //Assert 
            Assert.NotNull(shift);
            Assert.NotNull(shift.Task);
            Assert.NotNull(shift.Task.Instruction);
            Assert.NotNull(shift.Project);
            Assert.Equal(shiftId, shift.Id);
        }

        [Fact]
        public async Task GetShiftWithAvailabilitiesAsync_Should_Return_Null_When_Id_EMPTYGUIDS()
        {
            //Arrange
            //Act
            Shift shift = await shiftRepositoryTestMock.Object.GetShiftWithAvailabilitiesAsync(Guid.Empty);
            //Assert 
            Assert.Null(shift);
        }

        [Fact]
        public async Task GetShiftWithAvailabilitiesAsync_Should_Include_Related_Data_And_Filter_References()
        {
            //Arrange
            Guid shiftId = roosterPlannerContextMock.Object.Shifts.First().Id;
            //Act
            Shift shift = await shiftRepositoryTestMock.Object.GetShiftWithAvailabilitiesAsync(shiftId);
            //Assert 
            Assert.NotNull(shift);
            Assert.NotNull(shift.Task);
            Assert.NotNull(shift.Task.Instruction);
            Assert.NotNull(shift.Project);
            Assert.NotNull(shift.Availabilities);
            Assert.NotEmpty(shift.Availabilities);
            foreach (Availability availability in shift.Availabilities)
            {
                Assert.NotNull(availability.Participation);
                Assert.Null(availability.Participation.Availabilities);
                Assert.Null(availability.Shift);
            }

            Assert.Equal(shiftId, shift.Id);
            Assert.Null(shift.Task.Shifts);
        }

        [Fact]
        public async Task GetByProjectAndDateWithAvailabilitiesAsync_Should_Throw_Exception_When_Id_EMPTYGUIDS()
        {
            //Arrange
            DateTime dateTime = roosterPlannerContextMock.Object.Shifts.First().Date;
            //Act
            List<Shift> shift =
                await shiftRepositoryTestMock.Object.GetByProjectAndDateWithAvailabilitiesAsync(Guid.Empty, dateTime);
            //Assert 
            Assert.Null(shift);
        }

        [Fact]
        public async Task GetByProjectAndDateWithAvailabilitiesAsync_Should_Include_Related_Data_And_Filter_References()
        {
            //Arrange
            Shift s = roosterPlannerContextMock.Object.Shifts.First();
            Guid projectId = s.ProjectId;
            DateTime date = s.Date;
            //Act
            List<Shift> shifts =
                await shiftRepositoryTestMock.Object.GetByProjectAndDateWithAvailabilitiesAsync(projectId, date);
            //Assert 
            Assert.NotNull(shifts);
            Assert.NotEmpty(shifts);
            foreach (Shift shift in shifts)
            {
                Assert.NotNull(shift.Task);
                Assert.NotNull(shift.Task.Instruction);
                Assert.NotNull(shift.Availabilities);
                Assert.Equal(projectId, s.ProjectId);
                Assert.Equal(date, s.Date);
                Assert.Null(shift.Task.Shifts);
                foreach (Availability availability in shift.Availabilities)
                {
                    Assert.Null(availability.Shift);
                }
            }
        }
    }
}