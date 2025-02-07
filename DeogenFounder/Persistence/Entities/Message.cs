namespace DeogenFounder.Persistence.Entities;

public class Message : BaseEntity
{
    public int? FromId { get; set; }
    public Agent? From { get; set; }
    
    public bool ItWasSent { get; set; }
    
    public int? ToId { get; set; }
    public Agent? To { get; set; }

    public string Content { get; set; } = string.Empty;
    
    public int ChatId { get; set; }
    public Chat Chat { get; set; } = null!;
}
