using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;
using Moq;
using RoosterPlanner.Data.Context.Seed;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public class ProjectRepositoryTest : BaseUnitTest
    {
        private readonly Mock<ProjectRepository> projectRepositoryMock;

        public ProjectRepositoryTest()
        {
            projectRepositoryMock = new Mock<ProjectRepository>(roosterPlannerContextMock.Object)
            {
                CallBase = true
            };
        }

        private List<Project> CreateProjects()
        {
            List<Project> projects = ProjectSeed.Seed();
            return projects;
        }

        private List<Guid> CreateProjectObjectAddToRepo()
        {
            List<Guid> ids = new List<Guid>();
            List<Project> projects = CreateProjects();
            foreach (Project project in projects)
            {
                projectRepositoryMock.Object.Add(project);
                roosterPlannerContextMock.Object.SaveChanges();
                ids.Add(project.Id);
            }

            return ids;
        }

        [Fact]
        public void Can_Add_New_Entity()
        {
            //Arrange

            Project project = new Project(Guid.NewGuid());
            project.Name = DateTime.Now.ToString();

            //Act
            Project addedProject = projectRepositoryMock.Object.Add(project);
            int rowsAffected = roosterPlannerContextMock.Object.SaveChanges();

            //Assert
            Assert.NotNull(addedProject);
            Assert.NotEqual(Guid.Empty, addedProject.Id);
            Assert.True(addedProject.LastEditDate > DateTime.UtcNow.Date);
            Assert.True(rowsAffected > 0);
        }

        [Fact]
        public void Can_Update_Entity()
        {
            //Arrange
            Project project = roosterPlannerContextMock.Object.Projects.First();
            DateTime participationStart = project.ParticipationStartDate;
            string name = project.Name;

            //Act
            project.Name += "Updated";
            project.ParticipationStartDate = project.ParticipationStartDate.AddDays(7);
            Project updatedProject = projectRepositoryMock.Object.Update(project);
            int rowsAffected = roosterPlannerContextMock.Object.SaveChanges();

            //Assert
            Assert.NotNull(updatedProject);
            Assert.Equal(1, rowsAffected);
            Assert.Equal(updatedProject.Id, project.Id);
            Assert.True(updatedProject.ParticipationStartDate > participationStart);
            Assert.Null(updatedProject.PictureUri);
            Assert.True(updatedProject.ProjectTasks.Count==0);
            Assert.NotEqual(updatedProject.Name, name);
        }

        [Fact]
        public void Can_Get_New_Entity_ByKey()
        {
            //Arrange
            Guid projectId = Guid.NewGuid();
            Project project = new Project(projectId)
            {
                Name = $"Project_{DateTime.Now.ToString("yyyyddMM-HHmm")}",
                ParticipationStartDate = DateTime.Today.AddDays(-7),
                LastEditBy = "System",
                LastEditDate = DateTime.UtcNow.Date
            };

            //Act

            roosterPlannerContextMock.Object.Projects.Add(project);
            roosterPlannerContextMock.Object.SaveChanges();

            Project loadedProject = projectRepositoryMock.Object.Get(projectId);

            //Assert
            Assert.NotNull(loadedProject);
            Assert.Equal(projectId, loadedProject.Id);
            Assert.NotNull(loadedProject.Name);
            Assert.True(loadedProject.ParticipationStartDate > new DateTime(2019, 1, 1));
        }

        [Fact]
        public void Store_New_Unvalid_Entity_Throws_Exception()
        {
            //Arrange
            Project project = new Project
            {
                Name = $"Project_{DateTime.Now:yyyyddMM-HHmm}".PadRight(66, 'X'),
                ParticipationStartDate = DateTime.Today.AddDays(-7),
                LastEditBy = "System",
                LastEditDate = DateTime.UtcNow.Date
            };

            //Act

            //Assert
            Assert.Throws<ValidationException>(() => projectRepositoryMock.Object.AddOrUpdate(project));
        }

        [Fact]
        public async System.Threading.Tasks.Task Get_Should_Include_Related_Entities()
        {
            //Arrange
            //Act
            Project project = await projectRepositoryMock.Object.GetProjectDetailsAsync(ProjectSeed.Seed()[0].Id);

            //Assert    
            Assert.NotNull(project.PictureUri);
            Assert.NotNull(project.ProjectTasks);
        }

        [Fact]
        public async System.Threading.Tasks.Task Search_Should_Not_Accept_Null_filter()
        {
            //Arrange
            //Act
            //Assert  
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => projectRepositoryMock.Object.SearchProjectsAsync(null));
        }
    }
}