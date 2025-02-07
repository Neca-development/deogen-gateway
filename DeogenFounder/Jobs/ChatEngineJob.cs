using DeogenFounder.Common.Enums;
using DeogenFounder.Common.Interfaces;
using DeogenFounder.Common.Utils;
using DeogenFounder.Persistence;
using DeogenFounder.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace DeogenFounder.Jobs;

[DisallowConcurrentExecution]
public class ChatEngineJob(ContextDb ctx, IAgentService agentService, ILogger<ChatEngineJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogWarning("Starting chat engine job..");
        try
        {
            await Process();
            await TriggerPingMessage();
        }
        catch (Exception ex)
        {
            logger.LogError("Error in chat engine job\nMessage: {M}\nStackTrace: {S}", ex.Message, ex.StackTrace);
        }
    }

    private async Task TriggerPingMessage()
    {
        var chats = await ctx.Chats
            .Include(x => x.Messages)
            .ThenInclude(message => message.To)
            .Include(chat => chat.AgentChats)
            .ThenInclude(agentChat => agentChat.Agent)
            .ToListAsync();
        
        foreach (var item in chats)
        {
            var lastMsg = item.Messages.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
            if (lastMsg == null)
            {
                continue;
            }

            if (lastMsg.CreatedAt.AddMinutes(5) > DateTime.UtcNow)
            {
                continue;
            }

            if (lastMsg.To == null && lastMsg.Content.Last() == '?')
            {
                continue;
            }
            
            var manager = item.AgentChats.First(x => x.Agent.Type == "agent_manager").Agent;
            var xmlMsg = ChatMessageHelperUtil.ApplyXmlMessagePattern(
                "human",
                MessageType.Question,
                "Do you have any updates?");

            var response = "";
            while (response == string.Empty)
            {
                response = await agentService.SendMessage(
                    item.SessionId,
                    xmlMsg,
                    manager.ApiUrl,
                    manager.ApiKey);
            }
            
            var (recipient, text) = ChatMessageHelperUtil.ParseXmlAnswer(response);
            
            var msg = new Message
            {
                From = manager,
                ItWasSent = false,
                Content = text,
                Chat = item
            };

            if (recipient != "human")
            {
                var toAgent = await ctx.Agents.FirstAsync(x => 
                    x.AgentChats.Any(y => y.ChatId == item.Id) && x.Type == recipient);
                msg.To = toAgent;
            }
            
            await ctx.Messages.AddAsync(msg);
        }
        
        await ctx.SaveChangesAsync();
    }

    private async Task Process()
    {
        var notSentMessages = await ctx.Messages
            .Include(x => x.From)
            .Include(x => x.Chat)
            .Include(x => x.To)
            .Where(x => !x.ItWasSent && x.To != null)
            .ToArrayAsync();

        foreach (var item in notSentMessages)
        {
            var xmlMsg = ChatMessageHelperUtil.ApplyXmlMessagePattern(
                item.From == null ? "human" : item.From.Type,
                item.Content.Last() == '?' ? MessageType.Question : MessageType.Statement,
                item.Content);

            var response = "";

            while (response == string.Empty)
            {
                response = await agentService.SendMessage(
                    item.Chat.SessionId,
                    xmlMsg,
                    item.To!.ApiUrl,
                    item.To.ApiKey);
            }

            item.ItWasSent = true;

            var (recipient, text) = ChatMessageHelperUtil.ParseXmlAnswer(response);
            
            var msg = new Message
            {
                From = item.To,
                ItWasSent = false,
                Content = text,
                Chat = item.Chat
            };

            if (recipient != "human")
            {
                var toAgent = await ctx.Agents.FirstAsync(x => 
                    x.AgentChats.Any(y => y.ChatId == item.Chat.Id) && x.Type == recipient);
                msg.To = toAgent;
            }
            
            await ctx.Messages.AddAsync(msg);
        }
        
        await ctx.SaveChangesAsync();
    }
}
