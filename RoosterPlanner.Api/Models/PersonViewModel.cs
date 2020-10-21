using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graph;
using RoosterPlanner.Api.Models.Enums;

namespace RoosterPlanner.Api.Models
{
    public class PersonViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string UserRole { get; set; }

        public static PersonViewModel CreateVm(User user)
        {
            UserRole role = Enums.UserRole.None;
            if (user.AdditionalData.ContainsKey($"extension_4e6dae7dd1c74eac85fefc6da42e7b61_UserRole"))
            {
                Enum.TryParse(
                    user.AdditionalData[$"extension_4e6dae7dd1c74eac85fefc6da42e7b61_UserRole"]
                        .ToString(), out role);
            }

            return new PersonViewModel
            {
                Id = new Guid(user.Id),
                FirstName = user.GivenName,
                LastName = user.Surname,
                Email = user.Mail,
                StreetAddress = user.StreetAddress,
                PostalCode = user.PostalCode,
                City = user.City,
                Country = user.Country,
                UserRole = role.ToString(),
                DateOfBirth = user.AdditionalData["extension_4e6dae7dd1c74eac85fefc6da42e7b61_DateOfBirth"].ToString(),
                PhoneNumber = user.AdditionalData["extension_4e6dae7dd1c74eac85fefc6da42e7b61_PhoneNumber"].ToString()
            };
        }
    }
}