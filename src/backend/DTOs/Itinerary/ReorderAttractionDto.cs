namespace Swallow.DTOs.Itinerary;

public class ReorderAttractionDto
{
    public required int SourceDay { get; set; }
    public required int SourceIndex { get; set; }
    public required int DestinationDay { get; set; }
    public required int DestinationIndex { get; set; }
}