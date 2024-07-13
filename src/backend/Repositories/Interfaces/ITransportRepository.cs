using Swallow.Models;

namespace Swallow.Repositories.Interfaces;

public interface ITransportRepository
{
    Task<IEnumerable<Trip>> GetUpcomingTripsAsync(User user);
    Task<IEnumerable<TransportMode>> GetAllTransportModesAsync();
    Task<IEnumerable<TransportType>> GetAllTransportTypesAsync();
}