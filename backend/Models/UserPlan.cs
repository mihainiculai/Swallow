namespace Swallow.Models.DatabaseModels
{
    public class UserPlan
    {
        public int UserPlanId { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public int PlanId { get; set; }
        public virtual Plan Plan { get; set; } = null!;

        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public required int TripCount { get; set; } = 0;
    }
}
