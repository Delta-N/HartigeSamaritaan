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
        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Email 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the StreetAddress 
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the PostalCode 
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the City 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the DateOfBirth 
        /// </summary>
        public string DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNumber 
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the UserRole 
        /// </summary>
        public string UserRole { get; set; }

        /// <summary>
        /// Gets or sets the Nationality 
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// Gets or sets the NativeLanguage
        /// </summary>

        public string NativeLanguage { get; set; }

        /// <summary>
        /// Gets or sets the DutchProficiency
        /// </summary>
        public string DutchProficiency { get; set; }

        /// <summary>
        /// Gets or sets the PersonalRemark 
        /// </summary>
        public string PersonalRemark { get; set; }

        /// <summary>
        /// Gets or sets the ProfilePicture 
        /// </summary>
        public DocumentViewModel ProfilePicture { get; set; }

        /// <summary>
        /// Gets or sets the PushDisabled
        /// </summary>
        public bool PushDisabled { get; set; }

        /// <summary>
        /// Gets or sets the StaffRemark 
        /// </summary>
        public string StaffRemark { get; set; }

        /// <summary>
        /// Gets or sets the TermsofUserConsented
        /// </summary>
        public string TermsOfUseConsented { get; set; }

        /// <summary>
        /// Gets or sets the Certificates 
        /// </summary>
        public List<CertificateViewModel> Certificates { get; set; } = new();

        /// <summary>
        /// Creates a ViewModel from a User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a Microsoft.Graph.User from a ViewModel.
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
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
                AdditionalData = new Dictionary<string, object>
                {
                    {extension.DateOfBirthExtension, vm.DateOfBirth},
                    {extension.PhoneNumberExtension, vm.PhoneNumber},
                    {extension.NationalityExtension, vm.Nationality},
                    {extension.NativeLanguageExtention, vm.NativeLanguage},
                    {extension.DutchProficiencyExtention, vm.DutchProficiency},
                    {extension.TermsOfUseConsentedExtention, vm.TermsOfUseConsented}
                }
            };
            return user;
        }

        /// <summary>
        /// Creates a Person from a PersonViewModel.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
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
                DateOfBirth = vm.DateOfBirth,
                UserRole = vm.UserRole,
                PhoneNumber = vm.PhoneNumber,
                Nationality = vm.Nationality,
                LastEditDate = vm.LastEditDate,
                LastEditBy = vm.LastEditBy,
                RowVersion = vm.RowVersion,
                PersonalRemark = vm.PersonalRemark,
                PushDisabled = vm.PushDisabled,
                StaffRemark = vm.StaffRemark
            };
            if (vm.ProfilePicture == null) return person;

            person.ProfilePicture = DocumentViewModel.CreateDocument(vm.ProfilePicture);
            person.ProfilePictureId = person.ProfilePicture.Id;

            return person;
        }

        /// <summary>
        /// Creates a ViewModel from a Person.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
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
                DateOfBirth = person.DateOfBirth,
                UserRole = person.UserRole,
                Nationality = person.Nationality,
                PhoneNumber = person.PhoneNumber,
                LastEditDate = person.LastEditDate,
                LastEditBy = person.LastEditBy,
                RowVersion = person.RowVersion,
                PersonalRemark = person.PersonalRemark,
                PushDisabled = person.PushDisabled,
                StaffRemark = person.StaffRemark
            };

            if (person.ProfilePicture != null)
                vm.ProfilePicture = DocumentViewModel.CreateVm(person.ProfilePicture);

            return vm;
        }

        /// <summary>
        /// Creates a ViewModel from a User and a Person.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="person"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
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

            if (person.Certificates == null) return vmFromUser;
            foreach (Certificate certificate in person.Certificates)
                vmFromUser.Certificates.Add(CertificateViewModel.CreateVm(certificate));

            return vmFromUser;
        }
    }
}