using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class CertificateType : Entity
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        [Column(Order = 1)]
        [Required, MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Level
        /// </summary>
        [Column(Order = 2)]
        [MaxLength(256)]
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the Certificates
        /// </summary>
        public List<Certificate> Certificates { get; set; }

        /// <summary>
        /// Gets or sets the Requirements
        /// </summary>
        public List<Requirement> Requirements { get; set; }

        //constructor
        public CertificateType() : base(Guid.Empty)
        {
            Certificates = new List<Certificate>();
            Requirements = new List<Requirement>();

        }
        //constructor
        public CertificateType(Guid id) : base(id)
        {
            Certificates = new List<Certificate>();
            Requirements = new List<Requirement>();

        }
    }
}