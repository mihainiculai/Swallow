namespace Swallow.Utils.AttractionDataProviders
{
    public class TripAdvisorAttraction
    {
        public required string Name { get; set; }
        public required string TripAdvisorLink { get; set; }
        public required TripAdvisorAttractionDetails Details { get; set; }
    }

    public class TripAdvisorAttractionDetails
    {
        public double? Rating { get; set; }
        public int? Reviews { get; set; }
        public List<string> Categories { get; set; } = [];
        public string? VisitDuration { get; set; }
        public Dictionary<string, string> OpeningHours { get; set; } = [];
        public string? Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }
}
