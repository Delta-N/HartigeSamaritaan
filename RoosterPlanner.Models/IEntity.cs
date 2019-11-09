using System;

namespace RoosterPlanner.Models
{
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        Guid Id { get; }

        /// <summary>
        /// Gets or sets the LastEditBy.
        /// </summary>
        string LastEditBy { get; set; }

        /// <summary>
        /// Gets or sets the LastEditDate.
        /// </summary>
        DateTime LastEditDate { get; set; }

        /// <summary>
        /// Gets or sets the Rowversion.
        /// </summary>
        byte[] RowVersion { get; set; }

        /// <summary>
        /// Sets the Id of the entity when it is Guid.Empty.
        /// </summary>
        /// <param name="id">The Id to set.</param>
        void SetKey(Guid id);
    }
}
