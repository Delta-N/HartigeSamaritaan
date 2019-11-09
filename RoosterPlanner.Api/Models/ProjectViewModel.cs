using System;
using System.Collections.Generic;

namespace RoosterPlanner.Api.Models
{
    public class ProjectViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
