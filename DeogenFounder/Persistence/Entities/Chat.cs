namespace DeogenFounder.Persistence.Entities;

public class Chat : BaseEntity
{
    public string SessionId { get; set; } = string.Empty;
    public List<AgentChat> AgentChats { get; set; } = [];
    public List<Message> Messages { get; set; } = [];
}
