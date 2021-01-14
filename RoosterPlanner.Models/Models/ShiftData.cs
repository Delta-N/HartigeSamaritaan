using System;
using System.Collections.Generic;

namespace RoosterPlanner.Models.Models
{
    public class ShiftData
    {
        public List<Task> Tasks { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<string> StartTimes { get; set; }
        public List<string> EndTimes { get; set; }
        public List<int> ParticipantsRequired { get; set; }
    }
}