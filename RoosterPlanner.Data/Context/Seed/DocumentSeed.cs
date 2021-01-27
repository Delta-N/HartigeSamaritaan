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
                new(Guid.NewGuid())
                {
                    Name = "TermsOfService",
                    DocumentUri = "https://hackatonstoragedev.blob.core.windows.net/projectpicture/037efc9a-0836-4513-8ae3-282c597631c4.jfif",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                }
               
            };

            modelBuilder.Entity<Document>().HasData(documents.ToArray());
            return documents;
        }
    }
}