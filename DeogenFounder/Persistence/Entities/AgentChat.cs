namespace DeogenFounder.Persistence.Entities;

public class AgentChat : BaseEntity
{
    public int ChatId { get; set; }
    public Chat Chat { get; set; } = null!;
    
    public int AgentId { get; set; }
    public Agent Agent { get; set; } = null!;
}
