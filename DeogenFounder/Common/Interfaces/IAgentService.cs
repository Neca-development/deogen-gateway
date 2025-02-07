namespace DeogenFounder.Common.Interfaces;

public interface IAgentService
{
    public Task StartSession(string sessionId, string prompt, string url, string apiKey);
    public Task<string> SendMessage(string sessionId, string message, string url, string apiKey);
}
