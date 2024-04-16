using System.ComponentModel.DataAnnotations;

namespace Swallow.Models
{
    public class Country
    {
        public short CountryId { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [StringLength(2, MinimumLength = 2)]
        public required string Iso2 { get; set; }
        [StringLength(3, MinimumLength = 3)]
        public required string Iso3 { get; set; }
        [MaxLength(300)]
        public string? PictureUrl { get; set; }

        public virtual ICollection<City> Cities { get; } = [];
        public virtual ICollection<Currency> Currencies { get; } = [];
    }
}
