using System;

namespace RoosterPlanner.Common.Models
{
    public class AuthUser
    {
        //Claim objectidentifier
        public Guid UserId { get; set; }

        //Claim name
        public string Voornaam { get; set; }

        public string DisplayName { get; set; }

        //Claim emails
        public string Email { get; set; }

    }
}
