using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
