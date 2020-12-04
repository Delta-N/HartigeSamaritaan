namespace RoosterPlanner.Common.Config
{
    public class AzureBlobConfig
    {
        public static string ConfigSectionName => "AzureBlob";

        public string AzureBlobConnectionstring { get; set; }
    }
}