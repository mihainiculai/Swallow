namespace Swallow.Repositories.Interfaces
{
    public interface IAdvancedRepository<T, TId> : IRepository<T, TId> where T : class
    {
        Task<IEnumerable<T>> GetAllWithPaginationAsync(string? filterOn = null, string? filterQuery = null, string? orderBy = null, bool? isAscending = true, int? pageNumber = 1, int? pageSize = 1000);
    }
}
