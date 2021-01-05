using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class CertificateType : Entity
    {
        [Column(Order = 1)]
        [Required, MaxLength(256)]
        public string Name { get; set; }

        [Column(Order = 2)]
        [MaxLength(256)]
        public string Level { get; set; }

        public List<Certificate> Certificates { get; set; }



        public CertificateType() : base(Guid.Empty)
        {
            Certificates = new List<Certificate>();

        }

        public CertificateType(Guid id) : base(id)
        {
            Certificates = new List<Certificate>();

        }
    }
}