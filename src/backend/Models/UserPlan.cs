using System.ComponentModel.DataAnnotations.Schema;

namespace Swallow.Models
{
    public class UserPlan
    {
        public int UserPlanId { get; set; }

        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int PlanId { get; set; }
        public virtual Plan Plan { get; set; } = null!;

        public required int RemainingTrips { get; set; }
    }
}
