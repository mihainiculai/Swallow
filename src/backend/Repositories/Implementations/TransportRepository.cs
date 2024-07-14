using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations;

public class TransportRepository(ApplicationDbContext context) : ITransportRepository
{
    public async Task<IEnumerable<TransportMode>> GetAllTransportModesAsync()
    {
        return await context.TransportModes.ToListAsync();
    }
    
    public async Task<IEnumerable<TransportType>> GetAllTransportTypesAsync()
    {
        return await context.TransportTypes.ToListAsync();
    }
}