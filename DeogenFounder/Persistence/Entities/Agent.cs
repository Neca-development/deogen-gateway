namespace DeogenFounder.Persistence.Entities;

public class Agent : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
    // JSON
    public string Prompt { get; set; } = string.Empty;
    
    public string ApiUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;

    public List<AgentChat> AgentChats { get; set; } = [];
    public List<Message> FromMessages { get; set; } = [];
    public List<Message> ToMessages { get; set; } = [];
}
