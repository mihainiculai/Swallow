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

        public virtual ICollection<Attraction> Attractions { get; } = [];
        public virtual ICollection<Expense> Expenses { get; } = [];
        public virtual ICollection<CurrencyRate> CurrencyRates { get; } = [];
    }
}
