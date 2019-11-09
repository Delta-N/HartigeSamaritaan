using RoosterPlanner.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoosterPlanner.Api.Models
{
    public class ShiftViewModel
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime Date { get; set; }
        //TODO: Add shiftstatus
        //public ShiftStatus Status { get; set; }
    }
}
