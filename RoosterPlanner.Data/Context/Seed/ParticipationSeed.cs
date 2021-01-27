using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class ParticipationSeed : BaseSeed<Participation>
    {
        public ParticipationSeed(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        /// <summary>
        /// If the modelbuilder doesn't have any data of this enity, seed the data.
        /// </summary>
        /// <returns>A list with seeddata of this entity.</returns>
        public override List<Participation> Seed()
        {
            var participations = new List<Participation>
            {
               
            };

            modelBuilder.Entity<Participation>().HasData(participations.ToArray());

            return participations;
        }
    }
}