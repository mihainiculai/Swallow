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
            public required string city { get; set; }
            public required string country { get; set; }
            public required string iso2 { get; set; }
            public required string iso3 { get; set; }
            public required string lat { get; set; }
            public required string lng { get; set; }
        }

        public static async Task LoadWorldCities(ApplicationDbContext _context)
        {
            Dictionary<string, Country> countries = new Dictionary<string, Country>();

            using StreamReader reader = new StreamReader("Data/Resources/Cities.csv");
            using CsvReader csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            const int batchSize = 1000;
            int recordCount = 0;

            List<Country> batchList = new List<Country>();

            while (csv.Read())
            {
                CityRecord? record = csv.GetRecord<CityRecord>();
                if (record == null) continue;

                Country? country = countries.GetValueOrDefault(record.iso2);

                if (country == null)
                {
                    country = new Country
                    {
                        Name = record.country,
                        ISO2 = record.iso2,
                        ISO3 = record.iso3,
                    };
                    countries.Add(record.iso2, country);
                    batchList.Add(country);
                }

                country.Cities.Add(new City
                {
                    Name = record.city,
                    Latitude = decimal.Parse(record.lat),
                    Longitude = decimal.Parse(record.lng),
                    Country = country
                });

                recordCount++;

                if (recordCount % batchSize == 0)
                {
                    await _context.AddRangeAsync(batchList);
                    await _context.SaveChangesAsync();
                    batchList.Clear();
                }
            }

            if (batchList.Any())
            {
                await _context.AddRangeAsync(batchList);
                await _context.SaveChangesAsync();
            }

            _context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        public class CurrencyRecord
        {
            public required string country { get; set; }
            public required string name { get; set; }
            public required string code { get; set; }
            public required string symbol { get; set; }
        }

        public static async Task LoadCurrencies(ApplicationDbContext _context)
        {
            Dictionary<string, Country> countries = await _context.Countries.ToDictionaryAsync(c => c.Name);

            using var reader = new StreamReader("Data/Resources/Currencies.csv");
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            List<Currency> currencies = new List<Currency>();

            while (csv.Read())
            {
                CurrencyRecord? record = csv.GetRecord<CurrencyRecord>();
                if (record == null) continue;

                Country? country = countries.GetValueOrDefault(record.country);

                if (country == null)
                {
                    currencies.Add(new Currency
                    {
                        Name = record.name,
                        Code = record.code,
                        Symbol = record.symbol
                    });
                }
                else
                {
                    currencies.Add(new Currency
                    {
                        Name = record.name,
                        Code = record.code,
                        Symbol = record.symbol,
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