namespace Swallow.DTOs.Attraction
{
    public class GetAttractionsDto
    {
        public int AttractionId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<int> Categories { get; set; } = [];
        public decimal? Rating { get; set; }
        public int? Popularity { get; set; }
        public decimal? Price { get; set; }
        public string? PictureUrl { get; set; }
        public class ScheduleDto
        {
            public byte WeekdayId { get; set; }
            public required string WeekdayName { get; set; }
            public TimeOnly OpenTime { get; set; }
            public TimeOnly? CloseTime { get; set; }
        }
        public List<ScheduleDto> Schedules { get; set; } = [];
    }
}
