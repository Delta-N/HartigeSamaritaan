using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime? DeletedDateTime { get; set; } //Nodig?

        public CategoryViewModel Category { get; set; }

        public string Color { get; set; }

        public string DocumentUri { get; set; }
        //public List<Requirement> Requirements { get; set; } zodra requirements nodig zijn requirementviewmodel maken

        public string Description { get; set; }

        public byte[] RowVersion { get; set; }

        public static TaskViewModel CreateVm(Task task)
        {
            if (task != null)
            {
                return new TaskViewModel()
                {
                    Id = task.Id,
                    Name = task.Name,
                    Category = CategoryViewModel.CreateVm(task.Category),
                    Color = task.Color,
                    DocumentUri = task.DocumentUri,
                    //Requirements = RequirementViewModel.CreateVmFromList(task.Requirements),
                    Description = task.Description
                };
            }

            return null;
        }

        public static Task CreateTask(TaskViewModel taskViewModel)
        {
            if (taskViewModel != null)
            {
                return new Task(taskViewModel.Id)
                {
                    Name = taskViewModel.Name,
                    Category = CategoryViewModel.CreateCategory(taskViewModel.Category),
                    CategoryId = taskViewModel.Category.Id,
                    Color = taskViewModel.Color,
                    DocumentUri = taskViewModel.DocumentUri,
                    Description = taskViewModel.Description
                };
            }

            return null;
        }
    }
}