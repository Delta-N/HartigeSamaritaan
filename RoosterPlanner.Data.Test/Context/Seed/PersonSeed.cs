using System;
using System.Collections.Generic;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Context.Seed
{
    internal class PersonSeed
    {
        public static List<Person> Seed()
        {
     
            var persons = new List<Person>
            {
                new Person(Guid.Parse("62311d0a-d86e-4d78-b4bf-bc44e1ec8403"))
                {
                    FirstName = "John",
                    LastName = "Smith",
                    DateOfBirth = "01-01-1980",
                    City = "New York",
                    Email = "John@email.com",
                    Nationality = "Dutch",
                    Oid = Guid.Parse("62311d0a-d86e-4d78-b4bf-bc44e1ec8403"),
                    ProfilePictureId = Helper.ReturnRandomEntity(DocumentSeed.Seed()).Id
                }
            };

            persons.Add(new Person(Guid.Parse("60a8d986-4588-4e7f-b3d5-4023905397f1            "))
            {
                FirstName = "Stuart",
                LastName = "Riley",
                DateOfBirth = "01-01-1990",
                City = "Paris",
                Email = "Stuart@email.com",
                Nationality = "French",
                Oid = Guid.Parse("60a8d986-4588-4e7f-b3d5-4023905397f1"),
                ProfilePictureId = Helper.ReturnRandomEntity(DocumentSeed.Seed()).Id
            });

            persons.Add(new Person(Guid.Parse("b3dc933b-f334-4b95-9e66-8f638ed4c703"))
            {
                FirstName = "Elian",
                LastName = "Hamilton",
                DateOfBirth = "01-01-2010",
                City = "Moscow",
                Email = "Elian@email.com",
                Nationality = "Russian",
                Oid = Guid.Parse("b3dc933b-f334-4b95-9e66-8f638ed4c703"),
                ProfilePictureId = Helper.ReturnRandomEntity(DocumentSeed.Seed()).Id
            });

            persons.Add(new Person(Guid.Parse("3675c1c8-9d2f-4eb6-8751-0b5a4bd36e3b"))
            {
                FirstName = "Alfred",
                LastName = "Nelson",
                DateOfBirth = "01-01-2000",
                City = "Berlin",
                Email = "Alfred@email.com",
                Nationality = "German",
                Oid = Guid.Parse("3675c1c8-9d2f-4eb6-8751-0b5a4bd36e3b"),
                ProfilePictureId = Helper.ReturnRandomEntity(DocumentSeed.Seed()).Id
            });

            foreach (Person person in persons)
            {
                CertificateType type = Helper.ReturnRandomEntity(CertificateTypeSeed.Seed());
                person.Certificates.Add(new Certificate(Guid.NewGuid())
                {
                    CertificateTypeId = type.Id,
                    PersonId = person.Id,
                });
                
            }
            
            return persons;
        }
    }
}