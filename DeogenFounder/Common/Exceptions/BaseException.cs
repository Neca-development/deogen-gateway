namespace DeogenFounder.Common.Exceptions;

public abstract class BaseException : Exception
{
    public List<string> Errors { get; }
    public int StatusCode { get; protected set; }

    protected BaseException(string message, int statusCode) : base(message)
    {
        Errors = [message];
        StatusCode = statusCode;
    }

    protected BaseException(List<string> messages, int statusCode) : base(string.Join(", ", messages))
    {
        Errors = messages;
        StatusCode = statusCode;
    }
}
