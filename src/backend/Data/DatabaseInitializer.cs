using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Swallow.Models;
using System.Globalization;

namespace Swallow.Data
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }

    public class DatabaseInitializer(ApplicationDbContext context) : IDatabaseInitializer
    {
        public async Task InitializeAsync()
        {
            await context.Database.EnsureCreatedAsync();

            if (!context.Countries.Any())
            {
                await LoadWorldCities();
            }

            if (!context.Currencies.Any())
            {
                await LoadCurrencies();
            }
            
            if (!context.Roles.Any())
            {
                await LoadRoles();
            }
        }

        private async Task LoadWorldCities()
        {
            Dictionary<string, Country> countries = [];

            using StreamReader reader = new("Data/Resources/Cities.csv");
            using CsvReader csv = new(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            await using var transaction = await context.Database.BeginTransactionAsync();

            context.ChangeTracker.AutoDetectChangesEnabled = false;

            while (await csv.ReadAsync())
            {
                var record = csv.GetRecord<CityRecord>();
                if (record == null) continue;

                var country = countries.GetValueOrDefault(record.ISO2);

                if (country == null)
                {
                    country = new Country
                    {
                        Name = record.Country,
                        Iso2 = record.ISO2,
                        Iso3 = record.ISO3,
                    };
                    countries.Add(record.ISO2, country);
                    context.Countries.Add(country);
                }

                City city = new()
                {
                    Name = record.City,
                    Latitude = decimal.Parse(record.Lat, CultureInfo.InvariantCulture),
                    Longitude = decimal.Parse(record.Lng, CultureInfo.InvariantCulture),
                    Country = country
                };

                context.Cities.Add(city);
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        private async Task LoadCurrencies()
        {
            Dictionary<string, Country> countries = await context.Countries.ToDictionaryAsync(c => c.Name);

            using var reader = new StreamReader("Data/Resources/Currencies.csv");
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            List<Currency> currencies = [];

            while (await csv.ReadAsync())
            {
                var record = csv.GetRecord<CurrencyRecord>();
                if (record == null) continue;

                var country = countries.GetValueOrDefault(record.Country);

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

            context.ChangeTracker.AutoDetectChangesEnabled = false;

            await context.AddRangeAsync(currencies);
            await context.SaveChangesAsync();

            context.ChangeTracker.AutoDetectChangesEnabled = true;
        }
        
        private async Task LoadRoles()
        {
            var roles = new List<Role>
            {
                new() { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN" },
                new() { Id = Guid.NewGuid(), Name = "Premium", NormalizedName = "PREMIUM" }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        private class CityRecord
        {
            public required string City { get; set; }
            public required string Country { get; set; }
            public required string ISO2 { get; set; }
            public required string ISO3 { get; set; }
            public required string Lat { get; set; }
            public required string Lng { get; set; }
        }

        private class CurrencyRecord
        {
            public required string Country { get; set; }
            public required string Name { get; set; }
            public required string Code { get; set; }
            public required string Symbol { get; set; }
        }
    }

}
