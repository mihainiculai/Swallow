using Swallow.DTOs.City;
using Swallow.Models;

namespace Swallow.Repositories.Interfaces
{
    public interface ICityRepository
    {
        Task<City> GetByIdAsync(int id);
        Task UpdateAsync(CityDto cityDto);
        Task CalculateScoreAsync(City city);
        Task<IEnumerable<City>> TopCitiesAsync(int? count);
        IEnumerable<City> Search(string query);
    }
}