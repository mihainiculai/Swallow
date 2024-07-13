using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations;

public class TransportRepository(ApplicationDbContext context) : ITransportRepository
{
    public async Task<IEnumerable<Trip>> GetUpcomingTripsAsync(User user)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        var oneDayAgo = currentDate.AddDays(-1);

        var result = await context.Trips
            .Where(t => t.UserId == user.Id && t.EndDate >= oneDayAgo)
            .ToListAsync();

        return result;
    }
    
    public async Task<IEnumerable<TransportMode>> GetAllTransportModesAsync()
    {
        return await context.TransportModes.ToListAsync();
    }
    
    public async Task<IEnumerable<TransportType>> GetAllTransportTypesAsync()
    {
        return await context.TransportTypes.ToListAsync();
    }
}