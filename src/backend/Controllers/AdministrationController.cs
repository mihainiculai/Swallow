using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.Data;

namespace Swallow.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/administration")]
[ApiController]
public class AdministrationController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("statistics")]
    public IActionResult GetStatistics()
    {
        var numberOfUsers = context.Users.Count();
        var numberOfCountries = context.Countries.Count(c => c.Cities.Any(city => city.Attractions.Count != 0));
        var numberOfCities = context.Cities.Count(city => city.Attractions.Count != 0);
        var numberOfTrips = context.Trips.Count();
        
        var tripsPerMonth = new Dictionary<string, int>();
        for (var i = 0; i < 12; i++)
        {
            var month = DateTime.Now.AddMonths(-i).Month;
            var year = DateTime.Now.AddMonths(-i).Year;
            tripsPerMonth.Add($"{month}/{year}", context.Trips.Count(t => t.StartDate.Month == month && t.StartDate.Year == year));
        }
        
        return Ok(new
        {
            numberOfUsers,
            numberOfCountries,
            numberOfCities,
            numberOfTrips,
            tripsPerMonth
        });
    }
}