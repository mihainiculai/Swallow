using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Swallow.Models
{
    public class City
    {
        public int CityId { get; set; }

        public short CountryId { get; set; }
        public virtual Country Country { get; set; } = null!;

        [MaxLength(100)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [MaxLength(255)]
        public string? TripAdvisorUrl { get; set; }

        [Precision(10, 6)]
        public required decimal Latitude { get; set; }
        [Precision(10, 6)]
        public required decimal Longitude { get; set; }
        [MaxLength(255)]
        public string? PictureUrl { get; set; }
        [Precision(29, 2)]
        public decimal Score { get; set; } = 0;

        public virtual ICollection<Trip> Trips { get; } = [];
        public virtual ICollection<Attraction> Attractions { get; } = [];
    }
}
