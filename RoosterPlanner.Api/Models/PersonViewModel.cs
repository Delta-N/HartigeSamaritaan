using System;
using System.Collections.Generic;
using Microsoft.Graph;
using RoosterPlanner.Api.Models.Constants;
using RoosterPlanner.Models;
using RoosterPlanner.Models.Models.Enums;
using Person = RoosterPlanner.Models.Person;

namespace RoosterPlanner.Api.Models
{
    public class PersonViewModel : EntityViewModel
    {
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
        public string Nationality { get; set; }

        public string NativeLanguage { get; set; }
        public string DutchProficiency { get; set; }

        public string PersonalRemark { get; set; }

        public DocumentViewModel ProfilePicture { get; set; }
        public bool PushDisabled { get; set; }
        public string StaffRemark { get; set; }
        public string TermsOfUseConsented { get; set; }

        public List<CertificateViewModel> Certificates { get; set; } = new List<CertificateViewModel>();

        public static PersonViewModel CreateVmFromUser(User user, Extensions extension)
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
            if (user.Identities != null && personViewModel.Email == null)
                foreach (ObjectIdentity objectIdentity in user.Identities)
                    if (objectIdentity.SignInType == "emailAddress")
                        personViewModel.Email = objectIdentity.IssuerAssignedId;

            if (user.AdditionalData == null) return personViewModel;
            if (user.AdditionalData.ContainsKey(extension.UserRoleExtension))
            {
                Enum.TryParse(
                    user.AdditionalData[extension.UserRoleExtension]
                        .ToString(), out UserRole role);
                personViewModel.UserRole = role.ToString();
            }

            if (user.AdditionalData.ContainsKey(extension.DateOfBirthExtension))
                personViewModel.DateOfBirth = user
                    .AdditionalData[extension.DateOfBirthExtension]
                    .ToString();

            if (user.AdditionalData.ContainsKey(extension.PhoneNumberExtension))
                personViewModel.PhoneNumber = user
                    .AdditionalData[extension.PhoneNumberExtension]
                    .ToString();

            if (user.AdditionalData.ContainsKey(extension.NationalityExtension))
                personViewModel.Nationality = user
                    .AdditionalData[extension.NationalityExtension]
                    .ToString();

            if (user.AdditionalData.ContainsKey(extension.NativeLanguageExtention))
                personViewModel.NativeLanguage = user
                    .AdditionalData[extension.NativeLanguageExtention]
                    .ToString();

            if (user.AdditionalData.ContainsKey(extension.DutchProficiencyExtention))
                personViewModel.DutchProficiency = user
                    .AdditionalData[extension.DutchProficiencyExtention]
                    .ToString();
            
            if (user.AdditionalData.ContainsKey(extension.TermsOfUseConsentedExtention))
                personViewModel.TermsOfUseConsented = user
                    .AdditionalData[extension.TermsOfUseConsentedExtention]
                    .ToString();
            
            
            return personViewModel;
        }

        public static User CreateUser(PersonViewModel vm, Extensions extension)
        {
            if (vm == null || extension == null) 
                return null;
            
            User user = new User
            {
                Id = vm.Id.ToString(),
                GivenName = vm.FirstName,
                DisplayName = vm.FirstName,
                Surname = vm.LastName,
                StreetAddress = vm.StreetAddress,
                PostalCode = vm.PostalCode,
                City = vm.City,
                Country = vm.Country,
                AdditionalData = new Dictionary<string, object>
                {
                    {extension.DateOfBirthExtension, vm.DateOfBirth},
                    {extension.PhoneNumberExtension, vm.PhoneNumber},
                    {extension.NationalityExtension, vm.Nationality},
                    {extension.NativeLanguageExtention, vm.NativeLanguage},
                    {extension.DutchProficiencyExtention, vm.DutchProficiency},
                    {extension.TermsOfUseConsentedExtention, vm.TermsOfUseConsented}
                },
                
            };
            return user;

        }

        public static Person CreatePerson(PersonViewModel vm)
        {
            if (vm == null) return null;

            Person person = new Person(vm.Id)
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                StreetAddress = vm.StreetAddress,
                PostalCode = vm.PostalCode,
                City = vm.City,
                Country = vm.Country,
                DateOfBirth = vm.DateOfBirth,
                UserRole = vm.UserRole,
                PhoneNumber = vm.PhoneNumber,
                Nationality = vm.Nationality,
                LastEditDate = vm.LastEditDate,
                LastEditBy = vm.LastEditBy,
                RowVersion = vm.RowVersion,
                PersonalRemark = vm.PersonalRemark,
                PushDisabled = vm.PushDisabled,
                StaffRemark = vm.StaffRemark,
            };
            if (vm.ProfilePicture == null) return person;

            person.ProfilePicture = DocumentViewModel.CreateDocument(vm.ProfilePicture);
            person.ProfilePictureId = person.ProfilePicture.Id;

            return person;
        }

        public static PersonViewModel CreateVmFromPerson(Person person)
        {
            if (person == null) return null;
            PersonViewModel vm = new PersonViewModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                StreetAddress = person.StreetAddress,
                PostalCode = person.PostalCode,
                City = person.City,
                Country = person.Country,
                DateOfBirth = person.DateOfBirth,
                UserRole = person.UserRole,
                Nationality = person.Nationality,
                PhoneNumber = person.PhoneNumber,
                LastEditDate = person.LastEditDate,
                LastEditBy = person.LastEditBy,
                RowVersion = person.RowVersion,
                PersonalRemark = person.PersonalRemark,
                PushDisabled = person.PushDisabled,
                StaffRemark = person.StaffRemark,
            };

            if (person.ProfilePicture != null)
                vm.ProfilePicture = DocumentViewModel.CreateVm(person.ProfilePicture);

            return vm;
        }

        public static PersonViewModel CreateVmFromUserAndPerson(User user, Person person, Extensions extension)
        {
            if (user == null || person == null)
                return null;

            PersonViewModel vmFromUser = CreateVmFromUser(user, extension);

            vmFromUser.StaffRemark = person.StaffRemark;
            vmFromUser.PersonalRemark = person.PersonalRemark;
            vmFromUser.PushDisabled = person.PushDisabled;
            vmFromUser.LastEditBy = person.LastEditBy;
            vmFromUser.LastEditDate = person.LastEditDate;
            vmFromUser.RowVersion = person.RowVersion;

            if (person.ProfilePicture != null)
                vmFromUser.ProfilePicture = DocumentViewModel.CreateVm(person.ProfilePicture);
           
            if (person.Certificates != null)
                foreach (Certificate certificate in person.Certificates)
                    vmFromUser.Certificates.Add(CertificateViewModel.CreateVm(certificate));
                
            
            return vmFromUser;
        }
    }
}