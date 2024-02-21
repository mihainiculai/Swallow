using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Swallow.Models.DatabaseModels;
using System.Globalization;

namespace Swallow.Data
{
    public class DataInitializer
    {
        public class CityRecord
        {
            public required string City { get; set; }
            public required string Country { get; set; }
            public required string ISO2 { get; set; }
            public required string ISO3 { get; set; }
            public required string Lat { get; set; }
            public required string Lng { get; set; }
        }

        public static async Task LoadWorldCities(ApplicationDbContext _context)
        {

            Dictionary<string, Country> countries = [];

            using StreamReader reader = new("Data/Resources/Cities.csv");
            using CsvReader csv = new(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            using var transaction = await _context.Database.BeginTransactionAsync();

            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            while (csv.Read())
            {
                CityRecord? record = csv.GetRecord<CityRecord>();
                if (record == null) continue;

                Country? country = countries.GetValueOrDefault(record.ISO2);

                if (country == null)
                {
                    country = new Country
                    {
                        Name = record.Country,
                        Iso2 = record.ISO2,
                        Iso3 = record.ISO3,
                    };
                    countries.Add(record.ISO2, country);
                    _context.Countries.Add(country);
                }

                City city = new()
                {
                    Name = record.City,
                    Latitude = decimal.Parse(record.Lat),
                    Longitude = decimal.Parse(record.Lng),
                    Country = country
                };

                _context.Cities.Add(city);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        public class CurrencyRecord
        {
            public required string Country { get; set; }
            public required string Name { get; set; }
            public required string Code { get; set; }
            public required string Symbol { get; set; }
        }

        public static async Task LoadCurrencies(ApplicationDbContext _context)
        {
            Dictionary<string, Country> countries = await _context.Countries.ToDictionaryAsync(c => c.Name);

            using var reader = new StreamReader("Data/Resources/Currencies.csv");
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            List<Currency> currencies = [];

            while (csv.Read())
            {
                CurrencyRecord? record = csv.GetRecord<CurrencyRecord>();
                if (record == null) continue;

                Country? country = countries.GetValueOrDefault(record.Country);

                if (country == null)
                {
                    currencies.Add(new Currency
                    {
                        Name = record.Name,
                        Code = record.Code,
                        Symbol = record.Symbol
                    });
                }
                else
                {
                    currencies.Add(new Currency
                    {
                        Name = record.Name,
                        Code = record.Code,
                        Symbol = record.Symbol,
                        Country = country
                    });
                }
            }

            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            await _context.AddRangeAsync(currencies);
            await _context.SaveChangesAsync();

            _context.ChangeTracker.AutoDetectChangesEnabled = true;
        }
    }
}