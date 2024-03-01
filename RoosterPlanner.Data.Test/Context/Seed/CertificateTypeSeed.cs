using System;
using System.Collections.Generic;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class CertificateTypeSeed
    {
        public static List<CertificateType> Seed()
        {
            List<CertificateType> certificateTypes = new List<CertificateType>
            {
                new(Guid.Parse("a3ebca60-6bbf-4629-b498-d718f1f30ec0"))
                {
                    Name = "Biefstuk-capable",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },
                new(Guid.Parse("793403fd-ae5c-4a49-86df-203893db64c6"))
                {
                    Name = "Rijbewijs",
                    Level = "B",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },
                new(Guid.Parse("e181158b-9208-47b5-919d-c074363b786e"))
                {
                    Name = "Barcertificaat",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },
                new(Guid.Parse("337f1542-159b-4c88-8335-506e34a65754"))
                {
                    Name = "HACCP",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                }
            };

            return certificateTypes;
        }
    }
}