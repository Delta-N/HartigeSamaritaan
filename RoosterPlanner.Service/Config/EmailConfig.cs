namespace RoosterPlanner.Service.Config
{
    /// <summary>
    /// Class is used to bind Email configurationdetails to an object.
    /// (IOptionsPattern) 
    /// </summary>
    public class EmailConfig
    {
        public static string ConfigSectionName => "Email";

        public string Emailadres { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string SMTPadres { get; set; }

        public bool EnableSsl { get; set; }
    }
}