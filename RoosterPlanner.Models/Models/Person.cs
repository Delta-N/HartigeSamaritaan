using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RoosterPlanner.Models.Models
{
    public class Person : Entity
    {
        /// <summary>
        /// Gets or sets the Oid
        /// </summary>
        public Guid Oid { get; set; }

        /// <summary>
        /// Gets or sets the FirstName 
        /// </summary>

        [Required, MaxLength(256)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Participations 
        /// </summary>
        public List<Participation> Participations { get; set; }

        /// <summary>
        /// Gets or sets the Certificates 
        /// </summary>
        public List<Certificate> Certificates { get; set; }

        /// <summary>
        /// Gets or sets the Email 
        /// </summary>
        [NotMapped] public string Email { get; set; }

        /// <summary>
        /// Gets or sets the StreetAddress 
        /// </summary>
        [NotMapped] public string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the PostalCode 
        /// </summary>
        [NotMapped] public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the City 
        /// </summary>
        [NotMapped] public string City { get; set; }

        /// <summary>
        /// Gets or sets the DateOfBirth 
        /// </summary>
        [NotMapped] public string DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNumber
        /// </summary>
        [NotMapped] public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the UserRole
        /// </summary>
        [NotMapped] public string UserRole { get; set; }

        /// <summary>
        /// Gets or sets the Nationality 
        /// </summary>
        [NotMapped] public string Nationality { get; set; }

        /// <summary>
        /// Gets or sets the ProfilePictureId
        /// </summary>
        public Guid? ProfilePictureId { get; set; }

        /// <summary>
        /// Gets or sets the ProfilePicture 
        /// </summary>
        
        [ForeignKey("ProfilePictureId")] 
        public Document ProfilePicture { get; set; }

        /// <summary>
        /// Gets or sets the PersonalRemark
        /// </summary>
        [MaxLength(256)]
        public string PersonalRemark { get; set; }

        /// <summary>
        /// Gets or sets the StaffRemark
        /// </summary>
        [MaxLength(256)]
        public string StaffRemark { get; set; }

        /// <summary>
        /// Gets or sets the PushDisabled
        /// </summary>
        public bool PushDisabled { get; set; }

        //Constructor
        public Person() : base(Guid.Empty)
        {
            Participations = new List<Participation>();
            Certificates = new List<Certificate>();
        }

        //Constructor
        public Person(Guid id) : base(id)
        {
            Participations = new List<Participation>();
            Certificates = new List<Certificate>();
        }
    }
}
