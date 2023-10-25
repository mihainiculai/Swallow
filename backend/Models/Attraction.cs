using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Swallow.Models
{
    public class Attraction
    {
        public int AttractionId { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public virtual List<AttractionCategory> AttractionCategories { get; } = new();
        [Precision(10, 6)]
        public required decimal Latitude { get; set; }
        [Precision(10, 6)]
        public required decimal Longitude { get; set; }
        [MaxLength(255)]
        public required string Address { get; set; }
        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }
        [MaxLength(255)]
        public string? Website { get; set; }

        [Precision(10, 2)]
        public decimal? Price { get; set; }
        public short? CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; } = null!;

        [MaxLength(25)]
        public string? GooglePlaceId { get; set; }
        [MaxLength(255)]
        public string? GoogleMapsURL { get; set; }
        [MaxLength(255)]
        public string? TripAdvisorURL { get; set; }
        [MaxLength(255)]
        public string? PictureURL { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; } = null!;

        public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
        public virtual ICollection<ItineraryAttraction> ItineraryAttractions { get; } = new List<ItineraryAttraction>();
    }
}
