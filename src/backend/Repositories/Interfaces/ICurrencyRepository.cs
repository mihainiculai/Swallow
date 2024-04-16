using Swallow.Models;

namespace Swallow.Repositories.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetAllAsync();
        Task<Currency?> GetByCodeAsync(string code);
        Task<Currency> GetUSDAsync();
    }
}
