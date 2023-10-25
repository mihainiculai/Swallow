using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Swallow.Models;
using System.Globalization;

namespace Swallow.Data
{
    public class DataInitializer
    {
        public static async Task LoadWorldCities(DataContext _context)
        {
            using var reader = new StreamReader("Data/Resources/Cities.csv");
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            var records = csv.GetRecords<dynamic>().ToList();

            foreach (var record in records)
            {
                var countryName = record.country as string;
                var countryISO2 = record.iso2 as string;
                var countryISO3 = record.iso3 as string;

                if (countryName == null || countryISO2 == null || countryISO3 == null)
                {
                    continue;
                }

                var country = await _context.Countries
                                .Include(c => c.Cities)
                                .Where(c => c.ISO2 == countryISO2)
                                .FirstOrDefaultAsync();

                if (country == null)
                {
                    country = new Country
                    {
                        Name = countryName,
                        ISO2 = countryISO2,
                        ISO3 = countryISO3
                    };

                    _context.Countries.Add(country);
                    await _context.SaveChangesAsync();
                }

                var cityName = record.city as string;
                var cityLat = record.lat as string;
                var cityLng = record.lng as string;

                if (cityName == null || cityLat == null || cityLng == null || decimal.TryParse(cityLat, out decimal lat) == false || decimal.TryParse(cityLng, out decimal lng) == false)
                {
                    continue;
                }

                var city = await _context.Cities
                                .Where(c => c.Name == cityName && c.CountryId == country.CountryId)
                                .FirstOrDefaultAsync();

                if (city == null)
                {
                    city = new City
                    {
                        CountryId = country.CountryId,
                        Name = cityName,
                        Latitude = decimal.Parse(cityLat),
                        Longitude = decimal.Parse(cityLng)
                    };

                    country.Cities.Add(city);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public static async Task LoadCurrencies(DataContext _context)
        {
            using var reader = new StreamReader("Data/Resources/Currencies.csv");
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            var records = csv.GetRecords<dynamic>().ToList();

            foreach (var record in records)
            {
                var countryName = record.country as string;
                var country = await _context.Countries
                                .Include(c => c.Currencies)
                                .Where(c => c.Name == countryName)
                                .FirstOrDefaultAsync();

                var currencyName = record.name as string;
                var currencyCode = record.code as string;
                var currencySymbol = record.symbol as string;

                if (currencyName == null || currencyCode == null || currencySymbol == null)
                {
                    continue;
                }

                Currency currency = new Currency
                {
                    Name = currencyName,
                    Code = currencyCode,
                    Symbol = currencySymbol,
                };

                if (country != null)
                {
                    currency.CountryId = country.CountryId;
                }

                var oldCurrency = await _context.Currencies
                                    .Where(c => c.Code == currencyCode)
                                    .FirstOrDefaultAsync();

                if (oldCurrency != null)
                {
                    oldCurrency.Name = currencyName;
                    oldCurrency.Symbol = currencySymbol;
                    oldCurrency.CountryId = currency.CountryId;
                }
                else
                {
                    _context.Currencies.Add(currency);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}