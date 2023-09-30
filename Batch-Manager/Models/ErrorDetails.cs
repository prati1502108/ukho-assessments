namespace Batch_Manager.Models
{
    public class ErrorDetails
    {
        public string CorrelationId { get; set; } = string.Empty;
        public List<Error> Errors { get; set; } = new List<Error>();
    }
}
