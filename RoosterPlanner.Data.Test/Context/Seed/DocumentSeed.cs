using System;
using System.Collections.Generic;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class DocumentSeed
    {
        public static List<Document> Seed()
        {
            List<Document> documents = new List<Document>
            {
                new(Guid.Parse("c73be17e-909f-42dd-a22b-49400275909c"))
                {
                    Name = "Privacy Policy",
                    DocumentUri =
                        "https://hackatonstoragedev.blob.core.windows.net/projectpicture/037efc9a-0836-4513-8ae3-282c597631c4.jfif",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },
                new(Guid.Parse("ad6e3143-ba49-48b5-af06-97ef6cea08aa"))
                {
                    Name = "Profile picture",
                    DocumentUri = "https://linktoprofilepicture.com/id"
                },
                new(Guid.Parse("23fe35ed-3333-42a7-86d6-136803b5b42b"))
                {
                    Name = "Instruction",
                    DocumentUri = "https://linktoInstruction.com/id"
                },
            };

            return documents;
        }
    }
}