using System;

namespace RoosterPlanner.Api.Models
{
    public abstract class EntityViewModel
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the LastEditBy 
        /// </summary>
        public string LastEditBy { get; set; }

        /// <summary>
        /// Gets or sets the LastEditDate 
        /// </summary>
        public DateTime LastEditDate { get; set; }

        /// <summary>
        /// Gets or sets the RowVersion
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}