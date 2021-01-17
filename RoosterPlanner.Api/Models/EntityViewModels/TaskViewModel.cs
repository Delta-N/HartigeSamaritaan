﻿using System.Collections.Generic;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class TaskViewModel : EntityViewModel
    {
        public string Name { get; set; }

        public CategoryViewModel Category { get; set; }

        public string Color { get; set; }

        public DocumentViewModel Instruction { get; set; }

        public string Description { get; set; }
        public List<RequirementViewModel> Requirements { get; set; }

        public static TaskViewModel CreateVm(Task task)
        {
            if (task == null) return null;
            TaskViewModel vm = new TaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                Category = CategoryViewModel.CreateVm(task.Category),
                Color = task.Color,
                Description = task.Description,
                LastEditDate = task.LastEditDate,
                LastEditBy = task.LastEditBy,
                RowVersion = task.RowVersion
            };
            if (task.Instruction != null)
                vm.Instruction = DocumentViewModel.CreateVm(task.Instruction);

            vm.Requirements = new List<RequirementViewModel>();
            if (task.Requirements != null)
                foreach (Requirement requirement in task.Requirements)
                    vm.Requirements.Add(RequirementViewModel.CreateVm(requirement));

            return vm;
        }

        public static Task CreateTask(TaskViewModel taskViewModel)
        {
            if (taskViewModel == null)
                return null;

            Task task = new Task(taskViewModel.Id)
            {
                Name = taskViewModel.Name,
                Color = taskViewModel.Color,
                Description = taskViewModel.Description,
                LastEditDate = taskViewModel.LastEditDate,
                LastEditBy = taskViewModel.LastEditBy,
                RowVersion = taskViewModel.RowVersion
            };
            if (taskViewModel.Category != null)
            {
                task.CategoryId = taskViewModel.Category.Id;
                task.Category = CategoryViewModel.CreateCategory(taskViewModel.Category);
            }

            task.Requirements = new List<Requirement>();
            if (taskViewModel.Requirements != null)
                foreach (RequirementViewModel requirement in taskViewModel.Requirements)
                    task.Requirements.Add(RequirementViewModel.CreateRequirement(requirement));

            if (taskViewModel.Instruction == null) return task;
            task.Instruction = DocumentViewModel.CreateDocument(taskViewModel.Instruction);
            task.InstructionId = taskViewModel.Instruction.Id;

            return task;
        }
    }
}