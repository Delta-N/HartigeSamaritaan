namespace RoosterPlanner.Service.Config
{
    /// <summary>
    /// Class is used to bind Azure Blob configurationdetails to an object.
    /// (IOptionsPattern) 
    /// </summary>
    public class AzureBlobConfig
    {
        public static string ConfigSectionName => "AzureBlob";

        public string AzureBlobConnectionstring { get; set; }
    }
}