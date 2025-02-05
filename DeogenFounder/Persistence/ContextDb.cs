using DeogenFounder.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeogenFounder.Persistence;

public class ContextDb(DbContextOptions<ContextDb> opt) : DbContext(opt)
{
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<AgentChat> AgentChats => Set<AgentChat>();
}
