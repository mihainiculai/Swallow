using Swallow.Models;

namespace Swallow.Repositories.Interfaces;

public interface ITransportRepository
{
    Task<IEnumerable<TransportMode>> GetAllTransportModesAsync();
    Task<IEnumerable<TransportType>> GetAllTransportTypesAsync();
}