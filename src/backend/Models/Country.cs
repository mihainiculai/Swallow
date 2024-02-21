using System.ComponentModel.DataAnnotations;

namespace Swallow.Models.DatabaseModels
{
    public class Country
    {
        public short CountryId { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Iso2 { get; set; }
        public required string Iso3 { get; set; }
        public string? PhotoURL { get; set; }

        public virtual ICollection<City> Cities { get; } = new List<City>();
        public virtual ICollection<Currency> Currencies { get; } = new List<Currency>();
    }
}
