using System;
using Microsoft.EntityFrameworkCore;
using Moq;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Data.Context.Seed;

namespace RoosterPlanner.Data.Repositories
{
    public class BaseUnitTest
    
    {
        protected readonly Mock<RoosterPlannerContextTest> roosterPlannerContextMock;

        public BaseUnitTest()
        {
            roosterPlannerContextMock = CreateContextMock();
            roosterPlannerContextMock.Object.Projects.AddRange(ProjectSeed.Seed());
            roosterPlannerContextMock.Object.Persons.AddRange(PersonSeed.Seed());
            roosterPlannerContextMock.Object.Managers.AddRange(ManagerSeed.Seed());
            roosterPlannerContextMock.Object.Requirements.AddRange(RequirementSeed.Seed());
            roosterPlannerContextMock.Object.CertificateTypes.AddRange(CertificateTypeSeed.Seed());
            roosterPlannerContextMock.Object.Documents.AddRange(DocumentSeed.Seed());
            roosterPlannerContextMock.Object.Participations.AddRange(ParticipationSeed.Seed());
            roosterPlannerContextMock.Object.Tasks.AddRange(TaskSeed.Seed());
            roosterPlannerContextMock.Object.Categories.AddRange(CategorySeed.Seed());

            roosterPlannerContextMock.Object.SaveChanges();
        }

        private static DbContextOptions<RoosterPlannerContext> CreateContextOptions(string databaseName)
        {
            DbContextOptionsBuilder<RoosterPlannerContext> contextOptionsBuilder = new DbContextOptionsBuilder<
                RoosterPlannerContext>();
            contextOptionsBuilder.UseInMemoryDatabase(databaseName);
            return contextOptionsBuilder.Options;
        }

        public Mock<RoosterPlannerContextTest> CreateContextMock(string databaseName = null)
        {
            Mock<RoosterPlannerContextTest> mock =
                new Mock<RoosterPlannerContextTest>(CreateContextOptions(databaseName ?? Guid.NewGuid().ToString()))
                {
                    CallBase = true
                };

            return mock;
        }
    }
}