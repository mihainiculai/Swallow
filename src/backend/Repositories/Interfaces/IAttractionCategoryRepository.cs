using Swallow.Models;

namespace Swallow.Repositories.Interfaces
{
    public interface IAttractionCategoryRepository
    {
        Task<IEnumerable<AttractionCategory>> GetAllAsync();
        Task<AttractionCategory?> GetByNameAsync(string name);
        Task<List<AttractionCategory>> GetOrCreateAsync(IEnumerable<string> names);
        Task<AttractionCategory> GetOrCreateAsync(string name);
    }
}