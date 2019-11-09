using RoosterPlanner.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoosterPlanner.Api.Models
{
    public class Shift
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ShiftStatus Status { get; set; }
    }
}
