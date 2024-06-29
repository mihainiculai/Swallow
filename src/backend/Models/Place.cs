using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Swallow.Models;

public class Place
{
    public int PlaceId { get; set; }
    
    [MaxLength(255)]
    public required string Name { get; set; }
    [MaxLength(255)]
    public string? Address { get; set; }    
    
    [MaxLength(20)]
    public string? Phone { get; set; }
    [MaxLength(255)]
    public string? Website { get; set; }
    
    [Precision(3, 1)]
    public decimal? Rating { get; set; }
    public int? UserRatingsTotal { get; set; }

    [Precision(10, 6)]
    public decimal? Latitude { get; set; }
    [Precision(10, 6)]
    public decimal? Longitude { get; set; }
    
    [MaxLength(255)]
    public string? PictureUrl { get; set; }
    
    public string? GooglePlaceId { get; set; }
    public string? GoogleMapsUrl { get; set; }
    
    public virtual ICollection<TripToHotel> TripToHotels { get; } = [];
    public virtual ICollection<TripTransport> TripTransports { get; } = [];
}