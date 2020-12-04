namespace RoosterPlanner.Api.Models
{
    public class ErrorViewModel
    {
        public Type Type { get; set; }
        public string Message { get; set; }
    }

    public enum Type
    {
        Log,
        Warning,
        Error
    }
}