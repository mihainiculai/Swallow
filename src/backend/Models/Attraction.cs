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
        public virtual List<AttractionCategory> AttractionCategories { get; } = [];
        [Precision(10, 6)]
        public decimal? Latitude { get; set; }
        [Precision(10, 6)]
        public decimal? Longitude { get; set; }
        [MaxLength(255)]
        public string? Address { get; set; }
        [Phone]
        [MaxLength(20)]
        public string? Phone { get; set; }
        [MaxLength(255)]
        public string? Website { get; set; }
        [Precision(3, 1)]
        public decimal? Rating { get; set; }
        public int? Popularity { get; set; }
        public int? UserRatingsTotal { get; set; }
        [MaxLength(50)]
        public string? VisitDuration { get; set; }

        [Precision(10, 2)]
        public decimal? Price { get; set; }
        public short? CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; }

        [MaxLength(512)]
        public string? GooglePlaceId { get; set; }
        [MaxLength(255)]
        public string? GoogleMapsUrl { get; set; }
        [MaxLength(255)]
        public string? TripAdvisorUrl { get; set; }
        [MaxLength(255)]
        public string? PictureUrl { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; } = null!;

        public virtual ICollection<Schedule> Schedules { get; } = [];
        public virtual ICollection<ItineraryAttraction> ItineraryAttractions { get; } = [];
    }
}
