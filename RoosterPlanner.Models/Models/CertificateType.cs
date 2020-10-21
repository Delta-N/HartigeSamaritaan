﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models.Models
{
    public class CertificateType : Entity
    {
        [Column(Order = 1)]
        [Required, MaxLength(256)]
        public string Name { get; set; }

        [Column(Order = 2)]
        [Required, MaxLength(256)]
        public string Level { get; set; }

        public List<Certificate> Certificates { get; set; }

        public List<Requirement> Requirements { get; set; }

        public CertificateType() : base(Guid.Empty)
        {
            Certificates = new List<Certificate>();
            Requirements = new List<Requirement>();
        }

        public CertificateType(Guid id) : base(id)
        {
            Certificates = new List<Certificate>();
            Requirements = new List<Requirement>();
        }
    }
}