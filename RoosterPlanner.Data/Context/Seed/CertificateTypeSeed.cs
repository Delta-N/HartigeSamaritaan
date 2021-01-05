using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class CertificateTypeSeed : BaseSeed<CertificateType>
    {
        public CertificateTypeSeed(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        public override List<CertificateType> Seed()
        {
            List<CertificateType> certificateTypes = new List<CertificateType>
            {
                new(Guid.NewGuid())
                {
                    Name = "Biefstuk-capable",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                    
                },
                new(Guid.NewGuid())
                {
                    Name = "Rijbewijs",
                    Level = "B",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },
                new(Guid.NewGuid())
                {
                    Name = "Barcertificaat",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                },
                new(Guid.NewGuid())
                {
                    Name = "HACCP",
                    LastEditBy = "SYSTEM",
                    LastEditDate = DateTime.Now
                }
            };

            modelBuilder.Entity<CertificateType>().HasData(certificateTypes.ToArray());
            return certificateTypes;
        }
    }
}