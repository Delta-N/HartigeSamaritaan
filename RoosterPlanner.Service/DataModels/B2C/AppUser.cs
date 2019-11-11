using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoosterPlanner.Service.DataModels.B2C
{
    public class AppUser
    {
        [JsonProperty("objectId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid Id { get; set; }

        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; protected set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonIgnore]
        public string SignInName
        {
            get
            {
                if (SignInNames != null && SignInNames.Count != 0)
                {
                    var s = SignInNames.Where(x => x.Type.Equals("emailAddress", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    return s?.Value;
                }
                return null;
            }
        }

        [JsonProperty(PropertyName = "signInNames", Required = Required.Default)]
        public List<SignInName> SignInNames { get; set; }

        [JsonIgnore]
        public string Email { get; set; }

        [JsonProperty("otherMails")]
        public string[] OtherMails { get; set; }

        [JsonProperty("mobile")]
        public string PhoneNumber { get; set; }

        [JsonProperty("createdDateTime")]
        public DateTime? CreatedDateTime { get; set; }

        [JsonProperty("refreshTokensValidFromDateTime")]
        public DateTime? LastLoginAttempt { get; set; }

        public DateTime? LastSuccesfulLogin { get; set; }

        [JsonProperty("accountEnabled")]
        public bool IsActive { get; set; }

        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> AdditionalData { get; set; }

        //Constructor
        public AppUser()
        {
        }

        //Constructor - Overload
        public AppUser(string userPrincipalName)
        {
            this.UserPrincipalName = userPrincipalName;
        }
    }
}
