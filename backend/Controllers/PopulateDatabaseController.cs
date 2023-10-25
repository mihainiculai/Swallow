using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;

namespace Swallow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopulateDatabaseController : ControllerBase
    {
        private readonly DataContext _context;

        public PopulateDatabaseController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("worldcities")]
        public async Task<IActionResult> PopulateDatabase()
        {
            await DataInitializer.LoadWorldCities(_context);
            return Ok();
        }

        [HttpPost("currencies")]
        public async Task<IActionResult> PopulateCurrencies()
        {
            await DataInitializer.LoadCurrencies(_context);
            return Ok();
        }
        [HttpGet("currencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            var currencies = await _context.Currencies.ToListAsync();
            return Ok(currencies);
        }
    }
}
