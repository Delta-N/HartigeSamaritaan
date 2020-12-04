using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class TaskViewModel : EntityViewModel
    {
        public string Name { get; set; }

        public CategoryViewModel Category { get; set; }

        public string Color { get; set; }

        public string DocumentUri { get; set; }
        //public List<Requirement> Requirements { get; set; } zodra requirements nodig zijn requirementviewmodel maken

        public string Description { get; set; }

        public static TaskViewModel CreateVm(Task task)
        {
            if (task != null)
            {
                return new TaskViewModel
                {
                    Id = task.Id,
                    Name = task.Name,
                    Category = CategoryViewModel.CreateVm(task.Category),
                    Color = task.Color,
                    DocumentUri = task.DocumentUri,
                    //Requirements = RequirementViewModel.CreateVmFromList(task.Requirements),
                    Description = task.Description,
                    LastEditDate = task.LastEditDate,
                    LastEditBy = task.LastEditBy,
                    RowVersion = task.RowVersion
                };
            }

            return null;
        }

        public static Task CreateTask(TaskViewModel taskViewModel)
        {
            if (taskViewModel == null)
                return null;

            Task task = new Task(taskViewModel.Id)
            {
                Name = taskViewModel.Name,
                Color = taskViewModel.Color,
                DocumentUri = taskViewModel.DocumentUri,
                Description = taskViewModel.Description,
                LastEditDate = taskViewModel.LastEditDate,
                LastEditBy = taskViewModel.LastEditBy,
                RowVersion = taskViewModel.RowVersion
            };
            if (taskViewModel.Category == null)
                return task;

            task.CategoryId = taskViewModel.Category.Id;
            task.Category = CategoryViewModel.CreateCategory(taskViewModel.Category);

            return task;
        }
    }
}