using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    [Serializable]
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 0)]
        public virtual Guid Id { get; private set; }


        /// <summary>
        /// Gets or sets the LastEditBy.
        /// </summary>
        [Display(Name = "Laatst gewijzigd door")]
        [MaxLength(128)]
        [Column(Order = 97)]
        public string LastEditBy { get; set; }

        /// <summary>
        /// Gets or sets the LastEditDate.
        /// </summary>
        [Display(Name = "Laatst gewijzigd op")]
        [Column(TypeName = "datetime", Order = 98)]
        public DateTime LastEditDate { get; set; }

        /// <summary>
        /// Gets or sets the Rowversion.
        /// </summary>
        [Timestamp]
        [Column(Order = 99)]
        public byte[] RowVersion { get; set; }

        //Constructor
        public Entity() : this(Guid.Empty)
        {
        }

        //Constructor - Overload
        public Entity(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Sets the Id of the entity when it is Guid.Empty.
        /// </summary>
        /// <param name="id">The Id to set.</param>
        public void SetKey(Guid id)
        {
            if (Id == Guid.Empty)
                Id = id;
        }
    }
}
