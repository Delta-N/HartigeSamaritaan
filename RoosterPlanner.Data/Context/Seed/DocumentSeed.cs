using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class DocumentSeed : BaseSeed<Document>
    {
        public DocumentSeed(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        /// <summary>
        /// If the modelbuilder doesn't have any data of this enity, seed the data.
        /// </summary>
        /// <returns>A list with seeddata of this entity.</returns>
        public override List<Document> Seed()
        {
            List<Document> documents = new List<Document>
            {

            };

            modelBuilder.Entity<Document>().HasData(documents.ToArray());
            return documents;
        }
    }
}