using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class PersonSeed : BaseSeed<Person>
    {
        //Constructor
        public PersonSeed(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        /// <summary>
        /// If the modelbuilder doesn't have any data of this enity, seed the data.
        /// </summary>
        /// <returns>A list with seeddata of this entity.</returns>
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
