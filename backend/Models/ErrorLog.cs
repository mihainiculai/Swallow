namespace Swallow.Models
{
    public class ErrorLog
    {
        public int ErrorLogId { get; set; }
        public required string Message { get; set; }
        public required string StackTrace { get; set; }
        public required DateTime Date { get; set; }
    }
}
