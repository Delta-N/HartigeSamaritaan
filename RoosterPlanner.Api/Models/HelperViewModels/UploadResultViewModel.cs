namespace RoosterPlanner.Api.Models
{
    /// <summary>
    /// DTO for the results of a file upload.
    /// </summary>
    public class UploadResultViewModel
    {
        /// <summary>
        /// Gets or sets the path 
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets Succeeded 
        /// </summary>
        public bool Succeeded { get; set; }
    }
}