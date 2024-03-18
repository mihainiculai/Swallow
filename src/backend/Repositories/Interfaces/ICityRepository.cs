using Swallow.DTOs.City;
using Swallow.Models;

namespace Swallow.Repositories.Interfaces
{
    public interface ICityRepository
    {
        Task<City> GetByIdAsync(int id);
        Task UpdateAsync(CityDto cityDto);
    }
}