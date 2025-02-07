using DeogenFounder.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeogenFounder.Persistence;

public class ContextDb(DbContextOptions<ContextDb> opt) : DbContext(opt)
{
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<AgentChat> AgentChats => Set<AgentChat>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>()
            .HasOne(x => x.From)
            .WithMany(x => x.FromMessages)
            .HasForeignKey(x => x.FromId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Message>()
            .HasOne(x => x.To)
            .WithMany(x => x.ToMessages)
            .HasForeignKey(x => x.ToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
