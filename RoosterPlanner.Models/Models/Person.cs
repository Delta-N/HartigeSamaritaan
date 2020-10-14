﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RoosterPlanner.Models.Models;
using RoosterPlanner.Models.Models.Types;

namespace RoosterPlanner.Models
{
    public class Person : Entity
    {
        [Column(Order = 0)] public Guid Oid { get; set; }

        [Column(Order = 1)]
        [Required, MaxLength(256)]
        public string Name { get; set; }
        
        [Column(Order = 2 )]
        public PersonType Type { get; set; }

        public List<Participation> Participations { get; set; }
        public List<Certificate> Certificates { get; set; }
        

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