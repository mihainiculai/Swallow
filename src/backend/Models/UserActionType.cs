namespace Swallow.Models;

public class UserActionType
{
    public int UserActionTypeId { get; set; }
    public required string Name { get; set; }
    public required int Points { get; set; }
    
    public virtual ICollection<UserAction> UserActions { get; } = [];
}