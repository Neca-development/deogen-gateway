namespace DeogenFounder.Persistence.Entities;

public class Chat : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public List<AgentChat> AgentChats { get; set; } = [];
}
