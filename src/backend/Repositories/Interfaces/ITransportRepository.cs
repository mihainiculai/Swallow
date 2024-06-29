using Swallow.Models;

namespace Swallow.Repositories.Implementations;

public interface ITransportRepository
{
    Task<IEnumerable<TransportMode>> GetAllTransportModesAsync();
    Task<IEnumerable<TransportType>> GetAllTransportTypesAsync();
}