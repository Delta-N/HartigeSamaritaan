using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class PersonSeed : BaseSeed<Person>
    {
        //Constructor
        public PersonSeed(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        public override List<Person> Seed()
        {
            var persons = new List<Person>
            {
                
            };

            modelBuilder.Entity<Person>().HasData(persons.ToArray());

            return persons;
        }
    }
}
