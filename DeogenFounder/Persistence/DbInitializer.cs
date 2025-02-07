using DeogenFounder.Common.Configs;
using DeogenFounder.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeogenFounder.Persistence;

public static class DbInitializer
{
    public static async Task Run(ContextDb ctx, PromptConfig cfg)
    {
        if (!await ctx.Agents.AnyAsync())
        {
            foreach (var item in cfg.Items)
            {
                await ctx.Agents.AddAsync(new Agent
                {
                    Name = item.Name,
                    Type = item.Type,
                    Prompt = item.Prompt,
                    ApiUrl = item.ApiUrl,
                    ApiKey = item.ApiKey
                });
            }
        }
        
        await ctx.SaveChangesAsync();
    }
}
