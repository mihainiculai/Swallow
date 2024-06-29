using System.ComponentModel.DataAnnotations;

namespace Swallow.Models;

public class Hotel
{
    public int HotelId { get; set; }
    [MaxLength(255)]
    public required string Name { get; set; }
    [MaxLength(255)]
    public string? PictureUrl { get; set; }
    
    public string? GooglePlaceId { get; set; }
    public string? GoogleMapsUrl { get; set; }
    
    public virtual ICollection<TripToHotel> TripToHotels { get; } = [];
}