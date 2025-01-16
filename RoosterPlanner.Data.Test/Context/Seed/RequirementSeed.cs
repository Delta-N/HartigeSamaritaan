using System;
using System.Collections.Generic;
using RoosterPlanner.Models.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    public class RequirementSeed
    {
        public static List<Requirement> Seed()
        {
            List<Requirement> requirements = new List<Requirement>()
            {
                new(Guid.Parse("b260c367-22af-472f-8b5f-9bcca9bcf078"))
                {
                    TaskId = Guid.Parse("7c1046da-c3b1-498e-bd78-08eed4a53a56"), //chef,
                    CertificateTypeId = Guid.Parse("a3ebca60-6bbf-4629-b498-d718f1f30ec0") //biefstuk capable
                },
                new(Guid.Parse("7d43c3e5-0eec-42d9-a586-b7caf9984b9b"))
                {
                    TaskId = Guid.Parse("1316726a-a651-4cef-800d-a74746ca3d74"), //chauffeur
                    CertificateTypeId = Guid.Parse("793403fd-ae5c-4a49-86df-203893db64c6") //rijbewijs
                }
            };
            return requirements;
        }
    }
}