namespace DeogenFounder.Persistence.Entities;

public class Agent : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    // JSON
    public string Prompt { get; set; } = string.Empty;

    public List<AgentChat> AgentChats { get; set; } = [];
}
