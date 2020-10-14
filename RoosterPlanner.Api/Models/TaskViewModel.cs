using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Api.Models
{
    public class TaskViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime? DeletedDateTime { get; set; }

        public Category Category { get; set; }

        public string Color { get; set; }

        public string DocumentUri { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
