using System;

namespace RoosterPlanner.Api.Models
{
    public abstract class EntityViewModel
    {
        public Guid Id { get; set; }

        public string LastEditBy { get; set; }

        public DateTime LastEditDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}