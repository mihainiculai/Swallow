﻿namespace Swallow.DTOs.Attraction
{
    public class TripAdvisorAttraction
    {
        public required string Name { get; set; }
        public required int Popularity { get; set; }
        public required string TripAdvisorLink { get; set; }
        public required TripAdvisorAttractionDetails Details { get; set; }
    }

    public class TripAdvisorAttractionDetails
    {
        public List<string> Categories { get; set; } = [];
        public string? VisitDuration { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }
}
