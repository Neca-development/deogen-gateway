using DeogenFounder.Common.DTO;
using DeogenFounder.Common.DTO.Agents;
using DeogenFounder.Common.DTO.Chats;
using DeogenFounder.Common.Exceptions;
using DeogenFounder.Common.Interfaces;
using DeogenFounder.Common.Validators;
using DeogenFounder.Persistence;
using DeogenFounder.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeogenFounder.Controllers;

public class ChatsController(ContextDb ctx, IAgentService agentService) : BaseController
{
    [HttpGet("{chatId:int}")]
    public async Task<ApiResponse<ChatDto>> GetAllChatMessages(int chatId)
    {
        var chat = await ctx.Chats.FirstOrDefaultAsync(x => x.Id == chatId);
        if (chat == null)
        {
            throw new BadRequestException("Chat not found");
        }

        var messages = await ctx.Messages
            .Include(x => x.To)
            .Include(x => x.From)
            .Where(x => x.ChatId == chat.Id)
            .OrderByDescending(x => x.CreatedAt)
            .ToArrayAsync();

        var result = new ChatDto
        {
            Id = chat.Id,
            Messages = messages.Select(x => new ChatMessageDto
            {
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                To = x.To == null
                    ? null
                    : new AgentDto
                    {
                        Id = x.To.Id,
                        Name = x.To.Name,
                        Type = x.To.Type
                    },
                From = x.From == null
                    ? null
                    : new AgentDto
                    {
                        Id = x.From.Id,
                        Name = x.From.Name,
                        Type = x.From.Type
                    }
            }).ToArray()
        };

        return FormatResponse(result);
    }
    
    
    [HttpPost("Send")]
    public async Task SendMessage(SendMessageDto dto)
    {
        await EnsureIsValid<SendMessageValidator, SendMessageDto>(dto);
   
        var chat = await ctx.Chats
            .Include(x => x.AgentChats)
            .ThenInclude(x => x.Agent)
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(x => x.Id == dto.ChatId);
        
        if (chat == null)
        {
            throw new BadRequestException("Chat not found");
        }
        
        var manager = chat.AgentChats.First(x => x.Agent.Type == "agent_manager").Agent;
        chat.Messages.Add(new Message
        {
            To = manager,
            Content = dto.Message,
            Chat = chat
        });
        
        await ctx.SaveChangesAsync();
    }
    
    [HttpPost("Create")]
    public async Task<ApiResponse<int>> CreateChat(CreateChatDto dto)
    {
        await EnsureIsValid<CreateChatValidator, CreateChatDto>(dto);
        var agents = await ctx.Agents.Where(x => dto.AgentsIds.Contains(x.Id)).ToArrayAsync();
        var containsManager = false;
        var containsDeveloper = false;

        foreach (var item in agents)
        {
            if (item.Type == "agent_manager")
            {
                containsManager = true;
            }

            if (item.Type == "agent_developer")
            {
                containsDeveloper = true;
            }
        }

        if (!containsDeveloper || !containsManager)
        {
            throw new BadRequestException("Must contains as minimum manager and developer");
        }

        var agentTypes = agents.Select(x => x.Type);
        var typesToStr = string.Join(" | ", agentTypes);

        var chat = new Chat
        {
            SessionId = Guid.NewGuid().ToString("N"),
            AgentChats = agents.Select(x => new AgentChat { Agent = x }).ToList()
        };

        foreach (var item in agents)
        {
            var prompt = PreparePrompt(typesToStr, item.Prompt);
            await agentService.StartSession(chat.SessionId, prompt, item.ApiUrl, item.ApiKey);
        }
        
        await ctx.Chats.AddAsync(chat);
        await ctx.SaveChangesAsync();

        return FormatResponse(chat.Id);
    }

    private const string FromTypesMarker = "###fromTypes";
    private const string RecipientsMarker = "###recipient";

    private static string PreparePrompt(string types, string prompt)
    {
        prompt = prompt.Replace(FromTypesMarker, types);
        prompt = prompt.Replace(RecipientsMarker, types);
        return prompt;
    }
}
