using System.Text;
using DeogenFounder.Common.DTO.Agents;
using DeogenFounder.Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DeogenFounder.Services;

public class AgentService(HttpClient httpClient, ILogger<AgentService> logger) : IAgentService
{
    public async Task StartSession(string sessionId, string prompt, string url, string apiKey)
    {
        var req = new
        {
            prompt,
            sessionId
        };

        var httpMsg = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{url}/api/agent/start"),
            Content = new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json")
        };
        
        httpMsg.Headers.Add("X-API-KEY", apiKey);
        
        var response = await httpClient.SendAsync(httpMsg);
        if (!response.IsSuccessStatusCode)
        {
            var str = await response.Content.ReadAsStringAsync();
            logger.LogError("Status: {S}\nMessage: {M}", response.StatusCode, str);
            throw new Exception("Error starting session");
        }
    }
    
    public async Task<string> SendMessage(string sessionId, string message, string url, string apiKey)
    {
        var req = new
        {
            message = new JRaw(message),
            sessionId,
        };
        
        var json = JsonConvert.SerializeObject(req, new JsonSerializerSettings
        {
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
        });
        
        var httpMsg = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{url}/api/agent/message"),
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        
        httpMsg.Headers.Add("X-API-KEY", apiKey);
        
        var response = await httpClient.SendAsync(httpMsg);
        if (!response.IsSuccessStatusCode)
        {
            var msgHtStr = await httpMsg.Content.ReadAsStringAsync();
            var str = await response.Content.ReadAsStringAsync();
            logger.LogError("Status: {S}\nMessage: {M}", response.StatusCode, str);
            throw new Exception("Error starting session");
        }

        var content = await response.Content.ReadFromJsonAsync<AgentMessageResponse<string>>();
        if (content == null)
        {
            var str = await response.Content.ReadAsStringAsync();
            logger.LogError("Status: {S}\nMessage: {M}", response.StatusCode, str);
            throw new Exception("Error starting session");
        }
        
        return content.Data;
    }
}
