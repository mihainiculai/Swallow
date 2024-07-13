namespace Swallow.DTOs.Itinerary;

public class ItineraryDto
{
    public Guid TripId { get; set; }
    
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    
    public required int CityId { get; set; }
    public required ItineraryCityDto City { get; set; }
    public ItineraryTripToHotelDto? TripToHotel { get; set; }
    public byte TransportModeId { get; set; }
    
    public List<ItineraryDayDto> ItineraryDays { get; } = [];
    public List<ItineraryExpenseDto> Expenses { get; } = [];
}

public class ItineraryCityDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal Latitude { get; set; }
    public required decimal Longitude { get; set; }
    public string? PictureUrl { get; set; }
}

public class ItineraryTripToHotelDto
{
    public required ItineraryPlaceDto Place { get; set; }
    public ItineraryExpenseDto? Expense { get; set; }
}

public class ItineraryPlaceDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }    
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public decimal? Rating { get; set; }
    public int? UserRatingsTotal { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? PictureUrl { get; set; }
    public string? GooglePlaceId { get; set; }
    public string? GoogleMapsUrl { get; set; }
    public string? TripAdvisorUrl { get; set; }
}

public class ItineraryExpenseDto
{
    public Guid ExpenseId { get; set; }
    public short ExpenseCategoryId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? AttachmentUrl { get; set; }
    public decimal? Price { get; set; }
    public short? CurrencyId { get; set; }
}

public class ItineraryDayDto
{
    public int ItineraryDayId { get; set; }
    public DateOnly? Date { get; set; }
    public virtual ICollection<ItineraryAttractionDto> ItineraryAttractions { get; } = [];
}

public class ItineraryAttractionDto
{
    public int ItineraryAttractionId { get; set; }
    
    public required int Index { get; set; }
    public int AttractionId { get; set; }
    public required ItineraryPlaceDto Attraction { get; set; }
    public string? TicketsUrl { get; set; }
    public ItineraryExpenseDto? Expense { get; set; }
}
