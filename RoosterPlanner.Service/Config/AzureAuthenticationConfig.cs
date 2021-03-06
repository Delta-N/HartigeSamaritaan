﻿namespace RoosterPlanner.Service.Config
{
    
    /// <summary>
    /// Class is used to bind Azure B2C configurationdetails to an object.
    /// (IOptionsPattern) 
    /// </summary>
    public class AzureAuthenticationConfig
    {
        public static string ConfigSectionName => "AzureAuthentication";

        public string TenantId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string AzureTenantName { get; set; }//tenantname

        public string B2CExtentionApplicationId { get; set; }
        
        public string GraphApiScopes { get; set; }


    }
}
