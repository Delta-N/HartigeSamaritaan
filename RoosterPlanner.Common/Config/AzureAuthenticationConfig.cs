namespace RoosterPlanner.Common.Config
{
    public class AzureAuthenticationConfig
    {
        public static string ConfigSectionName { get { return "AzureAuthentication"; } }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }

        public string AzureTenantName { get; set; }
        public string AzureTenantId { get; set; }

        public string SignUpSignInPolicyId { get; set; }

        public string ResetPasswordPolicyId { get; set; }

        public string AzureADBaseUrl { get; set; }

        public string AzureADApiVersion { get; set; }

        public string AzureADTokenUrl { get; set; }

        public string GraphApiScopes { get; set; }

        public string GraphApiBaseUrl { get; set; }

        public string GraphB2cApplicationId { get; set; }

        public string ResourcePathUsers { get; set; }

        public string AzureADApplicationId { get; set; }

        public string AzureADClientSecret { get; set; }

        //Constructor
        public AzureAuthenticationConfig()
        {
        }
    }
}
