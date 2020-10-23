using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RoosterPlanner.Service.DataModels.B2C
{
    [JsonObject(MemberSerialization.OptIn)]
    public class B2cUser
    {
        [JsonIgnore]
        public Guid Id
        {
            get
            {
                if (String.IsNullOrEmpty(ObjectId))
                    return Guid.Empty;
                else
                    return Guid.Parse(ObjectId);
            }
        }

        //Do not serialize the property Id to Json.
        public bool ShouldSerializeId()
        {
            return false;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "objectId", Required = Required.Default)]
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or sets account enabled. true if the account is enabled; otherwise, false.
        /// This property is required when a user is created. Supports $filter.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "accountEnabled", Required = Required.Default)]
        public bool? AccountEnabled { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "creationType", Required = Required.Default)]
        public string CreationType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "createdDateTime", Required = Required.Default)]
        public DateTime? CreatedDateTime { get; private set; }

        //Do not serialize the property CreatedDateTime to Json.
        public bool ShouldSerializeCreatedDateTime()
        {
            return false;
        }

        /// <summary>
        /// Gets or sets display name. The name displayed in the address book for the user.
        /// This is usually the combination of the user's first name, middle initial and last name.
        /// This property is required when a user is created and it cannot be cleared during updates. Supports $filter and $orderby.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "displayName", Required = Required.Default)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets given name. The given name (first name) of the user. Supports $filter.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "givenName", Required = Required.Default)]
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets surname. The user's surname (family name or last name). Supports $filter.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "surname", Required = Required.Default)]
        public string Surname { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mailNickname", Required = Required.Default)]
        public string MailNickname { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "userPrincipalName", Required = Required.Default)]
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// Gets or sets password profile. Specifies the password profile for the user. The profile 
        /// contains the user's password. This property is required when a user is created.
        /// The password in the profile must satisfy minimum requirements as specified 
        /// by the passwordPolicies property. By default, a strong password is required.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "passwordProfile", Required = Required.Default)]
        public PasswordProfile PasswordProfile { get; set; }

        [JsonProperty(PropertyName = "signInNames", Required = Required.Default)]
        public List<SignInName> SignInNames { get; set; }

        /// <summary>
        /// Gets or sets street address. The street address of the user's place of business.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "streetAddress", Required = Required.Default)]
        public string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets postal code. The postal code for the user's postal address.
        /// The postal code is specific to the user's country/region.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "postalCode", Required = Required.Default)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets city. The city in which the user is located. Supports $filter.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "city", Required = Required.Default)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets country. The country/region in which the user is located; for example, "US" or "UK". Supports $filter.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "country", Required = Required.Default)]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets preferred language. The preferred language for the user.
        /// Should follow ISO 639-1 Code; for example "en-US".
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "preferredLanguage", Required = Required.Default)]
        public string PreferredLanguage { get; set; }

        /// <summary>
        /// Gets or sets the mobile phone number. Dutch formatting (ex. 06 12345678, 06-12345678, 070 1234556, 070-1234556, +31612345678).
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "mobile", Required = Required.Default)]
        public string MobilePhone { get; set; }

        //Do not serialize the property PreferredLanguage to Json.
        public bool ShouldSerializePreferredLanguage()
        {
            return false;
        }

        /// <summary>
        /// Gets or sets usage location.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "usageLocation", Required = Required.Default)]
        public string UsageLocation { get; set; }

        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> AdditionalData { get; set; }

        //Constructor
        public B2cUser()
        {
        }
    }

    // The type PasswordProfile.
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class PasswordProfile
    {
        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "password", Required = Required.Default)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets forceChangePasswordNextLogin.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "forceChangePasswordNextLogin", Required = Required.Default)]
        public bool ForceChangePasswordNextLogin { get; set; } = false;

        /// <summary>
        /// Gets or sets additional data.
        /// </summary>
        [JsonExtensionData(ReadData = true)]
        public IDictionary<string, object> AdditionalData { get; set; }

        //Constructor
        public PasswordProfile()
        {
        }
    }
}
