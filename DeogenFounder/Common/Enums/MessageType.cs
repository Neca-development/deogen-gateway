using System.ComponentModel;

namespace DeogenFounder.Common.Enums;

public enum MessageType
{
    Statement = 1,
    Question = 2
}

public static class MessageTypeExtensions
{
    public static string ConvertToString(this MessageType messageType)
    {
        return messageType switch
        {
            MessageType.Question => "question",
            MessageType.Statement => "statement",
            _ => throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null)
        };
    }
}
