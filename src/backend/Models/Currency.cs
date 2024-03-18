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
        public virtual ICollection<Attraction> Attractions { get; } = [];
        [JsonIgnore]
        public virtual ICollection<ItineraryAttraction> ItineraryAttractions { get; } = [];
        [JsonIgnore]
        public virtual ICollection<TripTransport> TripTransports { get; } = [];
        [JsonIgnore]
        public virtual ICollection<Expense> Expenses { get; } = [];
        [JsonIgnore]
        public virtual ICollection<CurrencyRate> CurrencyRates { get; } = [];
    }
}
