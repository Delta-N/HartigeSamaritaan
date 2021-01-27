namespace RoosterPlanner.Api.Models
{
    /// <summary>
    /// Used when returning a UnprocessableEntity
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the Message. 
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Indicates the severity of the Error.
    /// </summary>
    public enum Type
    {
        Log,
        Warning,
        Error
    }
}