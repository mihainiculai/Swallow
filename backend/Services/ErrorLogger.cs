using Swallow.Data;
using Swallow.Models.DatabaseModels;

namespace Swallow.Services
{
    public interface IErrorLogger
    {
        Task LogError(Exception exception);
    }

    public class ErrorLogger : IErrorLogger
    {
        private readonly ApplicationDbContext _context;

        public ErrorLogger(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogError(Exception exception)
        {
            var errorLog = new ErrorLog
            {
                Message = exception.Message,
                StackTrace = exception.StackTrace ?? "Unknown",
                Date = DateTime.UtcNow
            };

            _context.ErrorLogs.Add(errorLog);
            await _context.SaveChangesAsync();
        }
    }
}
