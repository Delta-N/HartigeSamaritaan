using System;
using Microsoft.Graph;
using RoosterPlanner.Api.Models.Enums;
using RoosterPlanner.Api.Models.Constants;

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
            PersonViewModel personViewModel = new PersonViewModel
            {
                Id = new Guid(user.Id),
                FirstName = user.GivenName,
                LastName = user.Surname,
                Email = user.Mail,
                StreetAddress = user.StreetAddress,
                PostalCode = user.PostalCode,
                City = user.City,
                Country = user.Country,
            };
            if (user.AdditionalData != null)
            {
                UserRole role;
                if (user.AdditionalData.ContainsKey(Extensions.UserRoleExtension))
                {
                    Enum.TryParse(
                        user.AdditionalData[Extensions.UserRoleExtension]
                            .ToString(), out role);
                    personViewModel.UserRole = role.ToString();
                }

                if (user.AdditionalData.ContainsKey(Extensions.DateOfBirthExtension))
                {
                    personViewModel.DateOfBirth = user
                        .AdditionalData[Extensions.DateOfBirthExtension]
                        .ToString();
                }

                if (user.AdditionalData.ContainsKey(Extensions.PhoneNumberExtension))
                {
                    personViewModel.PhoneNumber = user
                        .AdditionalData[Extensions.PhoneNumberExtension]
                        .ToString();
                }
            }
            return personViewModel;
        }
    }
}