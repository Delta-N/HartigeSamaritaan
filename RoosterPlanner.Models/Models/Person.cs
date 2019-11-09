using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoosterPlanner.Models
{
    public class Person : Entity
    {
        [Column(Order = 0)]
        public Guid Oid { get; set; }

        [Column(Order = 1)]
        [Required, MaxLength(256)]
        public string Name { get; set; }

        public List<Participation> Participations { get; set; }

        public List<ProjectPerson> ProjectsPersons { get; set; }

        //Constructor
        public Person() : base(Guid.Empty)
        {
        }

        //Constructor
        public Person(Guid id) : base(id)
        {
            Participations = new List<Participation>();
            ProjectsPersons = new List<ProjectPerson>();
        }
    }
}
