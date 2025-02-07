using DeogenFounder.Common.DTO;
using DeogenFounder.Common.DTO.Agents;
using DeogenFounder.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeogenFounder.Controllers;

public class AgentsController(ContextDb ctx) : BaseController
{
    [HttpGet("All")]
    public async Task<ApiResponse<AgentDto[]>> GetAll()
    {
        var agents = await ctx.Agents.ToArrayAsync();
        var response = agents.Select(x => new AgentDto
        {
            Id = x.Id,
            Name = x.Name,
            Type = x.Type
        });

        return FormatResponse(response.ToArray());
    }
}
