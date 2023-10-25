using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swallow.Data;
using Swallow.Models;
using System.Globalization;

namespace Swallow.Services
{
    public class ApiResponse
    {
        public required string Result { get; set; }
        public required string Time_Last_Update_Utc { get; set; }
        public required string Time_Next_Update_Utc { get; set; }
        public required Dictionary<string, decimal> Rates { get; set; }
    }

    public class UpdateCurrency : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;

        public UpdateCurrency(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Update, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void Update(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<DataContext>();

                var platformSettings = await _context.PlatformSettings.FirstOrDefaultAsync();
                
                if (platformSettings == null)
                {
                    return;
                }

                var nextCurrencyUpdate = platformSettings.NextCurrencyUpdate;
                
                if (nextCurrencyUpdate > DateTime.UtcNow)
                {
                    return;
                }

                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync("https://open.er-api.com/v6/latest/USD");
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);
                
                if (apiResponse == null)
                {
                    return;
                }

                var lastUpdateUtc = DateTime.ParseExact(apiResponse.Time_Last_Update_Utc, "ddd, dd MMM yyyy HH:mm:ss '+0000'", CultureInfo.InvariantCulture);

                if (lastUpdateUtc < nextCurrencyUpdate)
                {
                    return;
                }

                foreach (var rate in apiResponse.Rates)
                {
                    string currencyCode = rate.Key;
                    decimal rateToUSD = rate.Value;

                    var currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == currencyCode);

                    if (currency == null)
                    {
                        continue;
                    }

                    currency.CurrencyRates.Add(new CurrencyRate
                    {
                        RateToUSD = rateToUSD,
                        Date = lastUpdateUtc
                    });
                }

                var nextUpdateUtc = DateTime.ParseExact(apiResponse.Time_Next_Update_Utc, "ddd, dd MMM yyyy HH:mm:ss '+0000'", CultureInfo.InvariantCulture);
                platformSettings.NextCurrencyUpdate = nextUpdateUtc;
                _context.PlatformSettings.Update(platformSettings);

                await _context.SaveChangesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
