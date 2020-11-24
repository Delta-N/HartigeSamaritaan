using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class ShiftViewModel
    {
        public Guid Id { get; set; }
        public ProjectDetailsViewModel Project { get; set; }
        public TaskViewModel Task { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int ParticipantsRequired { get; set; }

        public static ShiftViewModel CreateVm(Shift shift)
        {
            if (shift != null)
            {
                return new ShiftViewModel()
                {
                    Id = shift.Id,
                    Project = ProjectDetailsViewModel.CreateVm(shift.Project),
                    Task = TaskViewModel.CreateVm(shift.Task),
                    Date = shift.Date,
                    StartTime = shift.StartTime,
                    EndTime = shift.EndTime,
                    ParticipantsRequired = shift.ParticipantsRequired
                };
            }

            return null;
        }

        public static Shift CreateShift(ShiftViewModel shiftViewModel)
        {
            if (shiftViewModel != null)
            {
                if (shiftViewModel.Project == null ||
                    shiftViewModel.Task == null)
                    return null;

                return new Shift(shiftViewModel.Id)
                {
                    StartTime = shiftViewModel.StartTime,
                    EndTime = shiftViewModel.EndTime,
                    Date = shiftViewModel.Date,
                    Task = TaskViewModel.CreateTask(shiftViewModel.Task),
                    Project = ProjectDetailsViewModel.CreateProject(shiftViewModel.Project),
                    ParticipantsRequired = shiftViewModel.ParticipantsRequired,
                    TaskId = shiftViewModel.Task.Id,
                    ProjectId = shiftViewModel.Project.Id
                };
            }

            return null;
        }
    }
}