using Microsoft.EntityFrameworkCore;

namespace Swallow.Models
{
    [PrimaryKey(nameof(AttractionId), nameof(WeekdayId))]
    public class Schedule
    {
        public int AttractionId { get; set; }
        public virtual Attraction Attraction { get; set; } = null!;
        public byte WeekdayId { get; set; }
        public virtual Weekday Weekday { get; set; } = null!;
        public TimeOnly OpenTime { get; set; }
        public TimeOnly? CloseTime { get; set; }
    }
}
