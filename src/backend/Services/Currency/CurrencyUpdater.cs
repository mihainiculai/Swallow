﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swallow.Data;
using Swallow.Models;
using System.Globalization;
using Swallow.DTOs.Currency;

namespace Swallow.Services.Currency;

public class CurrencyUpdater(ApplicationDbContext context, HttpClient httpClient) : ICurrencyUpdater
{
    private const string RequestUrl = "https://open.er-api.com/v6/latest/USD";
    private const string DatetimeFormat = "ddd, dd MMM yyyy HH:mm:ss '+0000'";
        
    private static DateTime ParseDateTime(string dateTime)
    {
        return DateTime.ParseExact(dateTime, DatetimeFormat, CultureInfo.InvariantCulture);
    }
        
    private static void UpdatePlatformSettings(ApplicationDbContext context, PlatformSettings platformSettings, DateTime lastUpdateUtc)
    {
        platformSettings.LastCurrencyUpdate = lastUpdateUtc;
        context.PlatformSettings.Update(platformSettings);
    }
    
    private static async Task ProcessApiResponseAsync(ApplicationDbContext context, CurrencyApiResponse apiResponse, DateTime lastUpdateUtc)
    {
        foreach (var rate in apiResponse.Rates)
        {
            var currencyCode = rate.Key;
            var rateToUsd = rate.Value;
            var currency = await context.Currencies.FirstOrDefaultAsync(c => c.Code == currencyCode);

            if (currency == null)
            {
                continue;
            }

            CurrencyRate currencyRate = new()
            {
                RateToUSD = rateToUsd,
                Date = lastUpdateUtc
            };

            currency.CurrencyRates.Add(currencyRate);
        }
    }
        
    public async Task UpdateCurrenciesAsync()
    {
        var platformSettings = await context.PlatformSettings.FirstOrDefaultAsync() ?? throw new Exception("Platform settings not found");

        var response = await httpClient.GetStringAsync(RequestUrl);
        var apiResponse = JsonConvert.DeserializeObject<CurrencyApiResponse>(response) ?? throw new Exception("API response is null");
        var lastUpdateUtc = ParseDateTime(apiResponse.TimeLastUpdateUtc);

        await ProcessApiResponseAsync(context, apiResponse, lastUpdateUtc);
        
        UpdatePlatformSettings(context, platformSettings, lastUpdateUtc);

        await context.SaveChangesAsync();
    }
}