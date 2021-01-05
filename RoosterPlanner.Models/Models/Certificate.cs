using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Certificate : Entity
    {
        [Display(Name = "Datum afgegeven")]
        [Column(TypeName = "datetime2", Order = 1)]
        public DateTime DateIssued { get; set; }
        
        [Display(Name = "Datum verlopen")]
        [Column(TypeName = "datetime2", Order = 2)]
        public DateTime? DateExpired { get; set; }

        [Column(Order = 3)]
        public Guid PersonId { get; set; }

        [Column(Order = 4)]
        public Guid? CertificateTypeId { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        
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