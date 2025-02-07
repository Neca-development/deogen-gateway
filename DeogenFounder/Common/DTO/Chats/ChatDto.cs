using DeogenFounder.Common.DTO.Agents;

namespace DeogenFounder.Common.DTO.Chats;

public class ChatDto
{
    public int Id { get; set; }
    public ChatMessageDto[] Messages { get; set; } = [];
}

public class ChatMessageDto
{
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public AgentDto? To { get; set; }
    public AgentDto? From { get; set; }
}
