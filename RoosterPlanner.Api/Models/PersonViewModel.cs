﻿using System;
using System.Collections.Generic;
using Microsoft.Graph;
using RoosterPlanner.Api.Models.Constants;
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

        public static PersonViewModel CreateVmFromUser(User user)
        {
            var personViewModel = new PersonViewModel
            {
                Id = new Guid(user.Id),
                FirstName = user.GivenName,
                LastName = user.Surname,
                Email = user.Mail,
                StreetAddress = user.StreetAddress,
                PostalCode = user.PostalCode,
                City = user.City,
                Country = user.Country
            };
            if (user.Identities != null && personViewModel.Email == null)
                foreach (var objectIdentity in user.Identities)
                    if (objectIdentity.SignInType == "emailAddress")
                        personViewModel.Email = objectIdentity.IssuerAssignedId;

            if (user.AdditionalData == null) return personViewModel;
            if (user.AdditionalData.ContainsKey(Extensions.UserRoleExtension))
            {
                Enum.TryParse(
                    user.AdditionalData[Extensions.UserRoleExtension]
                        .ToString(), out UserRole role);
                personViewModel.UserRole = role.ToString();
            }

            if (user.AdditionalData.ContainsKey(Extensions.DateOfBirthExtension))
                personViewModel.DateOfBirth = user
                    .AdditionalData[Extensions.DateOfBirthExtension]
                    .ToString();

            if (user.AdditionalData.ContainsKey(Extensions.PhoneNumberExtension))
                personViewModel.PhoneNumber = user
                    .AdditionalData[Extensions.PhoneNumberExtension]
                    .ToString();
            return personViewModel;
        }

        public static User CreateUser(PersonViewModel vm)
        {
            var user = new User
            {
                Id = vm.Id.ToString(),
                GivenName = vm.FirstName,
                Surname = vm.LastName,
                Mail = vm.Email,
                StreetAddress = vm.StreetAddress,
                PostalCode = vm.PostalCode,
                City = vm.City,
                Country = vm.Country,
                AdditionalData = new Dictionary<string, object>
                {
                    {Extensions.DateOfBirthExtension, vm.DateOfBirth},
                    {Extensions.PhoneNumberExtension, vm.PhoneNumber}
                }
            };
            return user;
        }

        public static RoosterPlanner.Models.Person CreatePerson(PersonViewModel vm)
        {
            return new RoosterPlanner.Models.Person(vm.Id)
            {
                firstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                StreetAddress = vm.StreetAddress,
                PostalCode = vm.PostalCode,
                City = vm.City,
                Country = vm.Country,
                DateOfBirth = vm.DateOfBirth,
                UserRole = vm.UserRole,
                PhoneNumber = vm.PhoneNumber
            };
        }

        public static PersonViewModel CreateVmFromPerson(RoosterPlanner.Models.Person person)
        {
            return new PersonViewModel
            {
             Id   = person.Id,
             FirstName = person.firstName,
             LastName = person.LastName,
             Email = person.Email,
             StreetAddress = person.StreetAddress,
             PostalCode = person.PostalCode,
             City = person.City,
             Country = person.Country,
             DateOfBirth = person.DateOfBirth,
             UserRole = person.UserRole,
             PhoneNumber = person.PhoneNumber
            };
        }
    }
}