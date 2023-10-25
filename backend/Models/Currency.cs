using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Swallow.Models
{
    public class Currency
    {
        public short CurrencyId { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(3)]
        public required string Code { get; set; }
        [MaxLength(5)]
        public required string Symbol { get; set; }

        public short? CountryId { get; set; }
        [JsonIgnore]
        public virtual Country? Country { get; set; }

        [JsonIgnore]
        public virtual ICollection<Attraction> Attractions { get; } = new List<Attraction>();
        [JsonIgnore]
        public virtual ICollection<ItineraryAttraction> ItineraryAttractions { get; } = new List<ItineraryAttraction>();
        [JsonIgnore]
        public virtual ICollection<TripTransport> TripTransports { get; } = new List<TripTransport>();
        [JsonIgnore]
        public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();
        [JsonIgnore]
        public virtual ICollection<CurrencyRate> CurrencyRates { get; } = new List<CurrencyRate>();
    }
}
