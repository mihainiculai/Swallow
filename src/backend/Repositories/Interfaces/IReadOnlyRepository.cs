using Swallow.Models.DatabaseModels;

namespace Swallow.Repositories.Interfaces
{
    public interface IReadOnlyRepository<T, TId> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(TId id);
    }
}
