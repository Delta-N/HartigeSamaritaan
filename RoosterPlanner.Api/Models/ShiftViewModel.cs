using System;
using System.Globalization;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ShiftViewModel
    {
        public Guid Id { get; set; }
        public ProjectDetailsViewModel Project { get; set; }
        public TaskViewModel Task { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int ParticipantsRequired { get; set; }

        public static ShiftViewModel CreateVm(Shift shift)
        {
            if (shift == null) 
                return null;
            
            ShiftViewModel vm = new ShiftViewModel
            {
                Id = shift.Id,
                Date = shift.Date,
                StartTime = shift.StartTime.ToString("hh\\:mm"),
                EndTime = shift.EndTime.ToString("hh\\:mm"),
                ParticipantsRequired = shift.ParticipantsRequired
            };

            if (shift.Project != null)
                vm.Project = ProjectDetailsViewModel.CreateVm(shift.Project);

            if (shift.Task != null)
                vm.Task = TaskViewModel.CreateVm(shift.Task);

            return vm;

        }

        public static Shift CreateShift(ShiftViewModel shiftViewModel)
        {
            if (shiftViewModel?.Project == null ||
                shiftViewModel.Task == null)
                return null;

            return new Shift(shiftViewModel.Id)
            {
                StartTime = TimeSpan.ParseExact(shiftViewModel.StartTime, "h\\:mm", CultureInfo.CurrentCulture),
                EndTime = TimeSpan.ParseExact(shiftViewModel.EndTime, "h\\:mm", CultureInfo.CurrentCulture),
                Date = shiftViewModel.Date,
                Task = TaskViewModel.CreateTask(shiftViewModel.Task),
                Project = ProjectDetailsViewModel.CreateProject(shiftViewModel.Project),
                ParticipantsRequired = shiftViewModel.ParticipantsRequired,
                TaskId = shiftViewModel.Task.Id,
                ProjectId = shiftViewModel.Project.Id
            };

        }
    }
}