﻿namespace RoosterPlanner.Common.Config
{
    public class AzureBlobConfig
    {
        public static string ConfigSectionName
        {
            get { return "AzureBlob"; }
        }

        public string AzureBlobConnectionstring { get; set; }
    }
}