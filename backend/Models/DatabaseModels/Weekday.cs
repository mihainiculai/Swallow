using Swallow.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations;

namespace Swallow.Models
{
    public class Weekday
    {
        public byte WeekdayId { get; set; }
        [MaxLength(10)]
        public required string Name { get; set; }

        public virtual ICollection<Schedule> Schedules { get; } = new List<Schedule>();
    }
}
