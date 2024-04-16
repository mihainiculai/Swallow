using Hangfire;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swallow.Data;
using Swallow.Models;
using System.Globalization;

namespace Swallow.Services.Currency
{
    public class CurrencyUpdater(ApplicationDbContext context, HttpClient httpClient) : ICurrencyUpdater
    {
        private const string REQUEST_URL = "https://open.er-api.com/v6/latest/USD";
        private const string DATETIME_FORMAT = "ddd, dd MMM yyyy HH:mm:ss '+0000'";
        
        public async Task UpdateCurrenciesAsync()
        {
            var platformSettings = await context.PlatformSettings.FirstOrDefaultAsync() ?? throw new Exception("Platform settings not found");

            if (!ShouldUpdateCurrency(platformSettings))
            {
                return;
            }

            var response = await httpClient.GetStringAsync(REQUEST_URL);
            var apiResponse = JsonConvert.DeserializeObject<CurrencyApiResponse>(response) ?? throw new Exception("API response is null");

            var lastUpdateUtc = ParseDateTime(apiResponse.TimeLastUpdateUtc);

            if (lastUpdateUtc <= platformSettings.LastCurrencyUpdate)
            {
                throw new Exception("Last update is not newer than the previous one");
            }

            await ProcessApiResponseAsync(context, apiResponse, lastUpdateUtc);

            var nextUpdateUtc = ParseDateTime(apiResponse.TimeNextUpdateUtc);

            UpdatePlatformSettings(context, platformSettings, lastUpdateUtc, nextUpdateUtc);

            await context.SaveChangesAsync();

            BackgroundJob.Schedule(() => UpdateCurrenciesAsync(), nextUpdateUtc.AddMinutes(1));
        }

        private static void UpdatePlatformSettings(ApplicationDbContext context, PlatformSettings platformSettings, DateTime lastUpdateUtc, DateTime nextUpdateUtc)
        {
            platformSettings.LastCurrencyUpdate = lastUpdateUtc;
            platformSettings.NextCurrencyUpdate = nextUpdateUtc;

            context.PlatformSettings.Update(platformSettings);
        }

        private static bool ShouldUpdateCurrency(PlatformSettings platformSettings)
        {
            var nextCurrencyUpdate = platformSettings.NextCurrencyUpdate;

            return DateTime.UtcNow >= nextCurrencyUpdate;
        }

        private static async Task ProcessApiResponseAsync(ApplicationDbContext context, CurrencyApiResponse apiResponse, DateTime lastUpdateUtc)
        {
            foreach (var rate in apiResponse.Rates)
            {
                string currencyCode = rate.Key;
                decimal rateToUSD = rate.Value;

                var currency = await context.Currencies.FirstOrDefaultAsync(c => c.Code == currencyCode);

                if (currency == null)
                {
                    continue;
                }

                CurrencyRate currencyRate = new()
                {
                    RateToUSD = rateToUSD,
                    Date = lastUpdateUtc
                };

                currency.CurrencyRates.Add(currencyRate);
            }
        }

        private static DateTime ParseDateTime(string dateTime)
        {
            return DateTime.ParseExact(dateTime, DATETIME_FORMAT, CultureInfo.InvariantCulture);
        }

        private class CurrencyApiResponse
        {
            [JsonProperty("time_last_update_utc")]
            public required string TimeLastUpdateUtc { get; set; }
            [JsonProperty("time_next_update_utc")]
            public required string TimeNextUpdateUtc { get; set; }
            [JsonProperty("rates")]
            public required Dictionary<string, decimal> Rates { get; set; }
        }
    }
}
