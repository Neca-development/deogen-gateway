namespace DeogenFounder.Common.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string message) : base(message, 400)
    {
    }

    public BadRequestException(List<string> messages) : base(messages, 400)
    {
    }
}
