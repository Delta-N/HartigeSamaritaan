using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RoosterPlanner.Models.Models
{
    public class Certificate : Entity
    {
        /// <summary>
        /// Gets or sets the DateIssued
        /// </summary>
        [Display(Name = "Datum afgegeven")]
        [Column(TypeName = "datetime2", Order = 1)]
        public DateTime DateIssued { get; set; }

        /// <summary>
        /// Gets or sets the DateExpired
        /// </summary>
        [Display(Name = "Datum verlopen")]
        [Column(TypeName = "datetime2", Order = 2)]
        public DateTime? DateExpired { get; set; }

        /// <summary>
        /// Gets or sets the PersonId
        /// </summary>
        public Guid PersonId { get; set; }

        /// <summary>
        /// Gets or sets the CertificateTypeId
        /// </summary>
        public Guid? CertificateTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Person
        /// </summary>
        [ForeignKey("PersonId")]
        public Person Person { get; set; }

        /// <summary>
        /// Gets or sets the CertificateType
        /// </summary>
        [ForeignKey("CertificateTypeId")]
        public CertificateType CertificateType { get; set; }
        
        
        
        //Constructor
        public Certificate() : base(Guid.Empty)
        {
        }
        
        //Constructor
        public Certificate(Guid id) : base(id)
        {
        }
    }
}
