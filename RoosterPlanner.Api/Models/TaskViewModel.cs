using System;
using RoosterPlanner.Models;

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
