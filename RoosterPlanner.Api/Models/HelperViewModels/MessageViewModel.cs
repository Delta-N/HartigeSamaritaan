namespace RoosterPlanner.Api.Models
{
    /// <summary>
    /// Used when sending emails
    /// </summary>
    public class MessageViewModel
    {
        /// <summary>
        /// Gets or sets the subject 
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body of the message 
        /// </summary>
        public string Body { get; set; }
    }
}