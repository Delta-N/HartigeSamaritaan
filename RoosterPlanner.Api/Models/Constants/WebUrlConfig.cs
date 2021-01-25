namespace RoosterPlanner.Api.Models.Constants
{
    /// <summary>
    /// Class is used to bind the URL of the webappto an object.
    /// Used in sending an email
    /// </summary>
    public class WebUrlConfig
    {
        public static string ConfigSectionName => "WebUrl";

        public string Url { get; set; }
    }
}