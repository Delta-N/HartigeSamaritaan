using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RoosterPlanner.Common;
using RoosterPlanner.Data.Context;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    [TestClass]
    public class ProjectRepositoryTest : DatabaseContext
    {
        private Project CreateProjectObject(Guid id, string name = null)
        {
            Project project = new Project(id) {
                Name = name ?? $"Project_{DateTime.Now.ToString("yyyyddMM-HHmm")}",
                StartDate = DateTime.Today.AddDays(-7),
                LastEditBy = "System",
                LastEditDate = DateTime.UtcNow.Date
            };
            return project;
        }

        [ClassInitialize()]
        public static void ClassInit(TestContext context) => Init(context);

        [TestMethod]
        public void Can_Add_New_Entity()
        {
            //Arrange
            Project project = new Project {
                Name = $"Project_{DateTime.Now.ToString("yyyyddMM-HHmm")}",
                StartDate = DateTime.Today.AddDays(-7),
                LastEditBy = "System",
                LastEditDate = DateTime.UtcNow.Date
            };

            MockRepository mockRepo = new MockRepository(MockBehavior.Default);
            Mock<ILogger> loggerMock = mockRepo.Create<ILogger>();

            //Act
            Project addedProject = null;
            int rowsAffected = 0;
            using (RoosterPlannerContext context = GetRoosterPlannerContext(connectionStringsConfig))
            {
                ProjectRepository projectRepo = new ProjectRepository(context, loggerMock.Object);
                addedProject = projectRepo.AddOrUpdate(project);

                rowsAffected = context.SaveChanges();
            }


            //Assert
            Assert.IsNotNull(addedProject);
            Assert.AreNotEqual<Guid>(Guid.Empty, addedProject.Id);
            Assert.IsTrue(addedProject.LastEditDate > DateTime.UtcNow.Date);
            Assert.AreEqual<int>(1, rowsAffected);
        }

        [TestMethod]
        public void Can_Get_New_Entity_ByKey()
        {
            //Arrange
            Guid projectId = Guid.NewGuid();
            Project project = new Project(projectId) {
                Name = $"Project_{DateTime.Now.ToString("yyyyddMM-HHmm")}",
                StartDate = DateTime.Today.AddDays(-7),
                LastEditBy = "System",
                LastEditDate = DateTime.UtcNow.Date
            };

            MockRepository mockRepo = new MockRepository(MockBehavior.Default);
            Mock<ILogger> loggerMock = mockRepo.Create<ILogger>();

            //Act
            Project loadedProject = null;
            using (RoosterPlannerContext context = GetRoosterPlannerContext(connectionStringsConfig))
            {
                context.Projects.Add(project);
                context.SaveChanges();
            }

            using (RoosterPlannerContext context = GetRoosterPlannerContext(connectionStringsConfig))
            {
                ProjectRepository projectRepo = new ProjectRepository(context, loggerMock.Object);
                loadedProject = projectRepo.Get(projectId);
            }

            //Assert
            Assert.IsNotNull(loadedProject);
            Assert.AreEqual<Guid>(projectId, loadedProject.Id);
            Assert.IsNotNull(loadedProject.Name);
            Assert.IsTrue(loadedProject.StartDate > new DateTime(2019, 1, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Store_New_Unvalid_Entity_Throws_Exception()
        {
            //Arrange
            Project project = new Project {
                Name = $"Project_{DateTime.Now.ToString("yyyyddMM-HHmm")}".PadRight(66, 'X'),
                StartDate = DateTime.Today.AddDays(-7),
                LastEditBy = "System",
                LastEditDate = DateTime.UtcNow.Date
            };

            MockRepository mockRepo = new MockRepository(MockBehavior.Default);
            Mock<ILogger> loggerMock = mockRepo.Create<ILogger>();
            loggerMock.Setup(s => s.LogException(It.IsNotNull<ValidationException>(), It.IsNotNull<Dictionary<string, string>>()));

            //Act
            Project savedProject = null;
            using (RoosterPlannerContext context = GetRoosterPlannerContext(connectionStringsConfig))
            {
                ProjectRepository projectRepo = new ProjectRepository(context, loggerMock.Object);
                savedProject = projectRepo.AddOrUpdate(project);
            }

            //Assert
            Assert.IsNull(savedProject);
        }
    }
}
