namespace RoosterPlanner.Common.Config
{
    public class AzureAuthenticationConfig
    {
        public static string ConfigSectionName { get { return "AzureAuthentication"; } }

        public string TenantId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string AzureTenantName { get; set; }//tenentname

        public string AzureADApiVersion { get; set; }//

        public string AzureADTokenUrl { get; set; }

        public string AzureADBaseUrl { get; set; }//

        public string B2CExtentionApplicationId { get; set; }

        public string GraphApiBaseUrl { get; set; }

        public string GraphApiScopes { get; set; }

        public string GraphB2cApplicationId { get; set; }

        public string SignUpSignInPolicyId { get; set; }

        public string ResetPasswordPolicyId { get; set; }

        public string ResourcePathUsers { get; set; }

        public string RedirectUri { get; set; }

        //Constructor
        public AzureAuthenticationConfig()
        {
        }
    }
}
