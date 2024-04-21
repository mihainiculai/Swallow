namespace Swallow.Models;

public class UserAction
{
    public int UserActionId { get; set; }
    
    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    public int AttractionId { get; set; }
    public virtual Attraction Attraction { get; set; } = null!;
    
    public required int UserActionTypeId { get; set; }
    public virtual UserActionType UserActionType { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}