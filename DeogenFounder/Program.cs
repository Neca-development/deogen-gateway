using System.Text.Json.Serialization;
using DeogenFounder.Common.Configs;
using DeogenFounder.Common.Interfaces;
using DeogenFounder.Extensions;
using DeogenFounder.Jobs;
using DeogenFounder.Middlewares;
using DeogenFounder.Persistence;
using DeogenFounder.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });

builder.Services.AddSwagger();
builder.Services.AddDbContext<ContextDb>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresqlContext")));

builder.Services.Configure<PromptConfig>(config.GetSection("Prompts"));
builder.Services.AddHttpClient<IAgentService, AgentService>();
builder.Services.Configure<QuartzOptions>(opt =>
{
    opt.SchedulerId = "core";
    opt.SchedulerName = "core scheduler";
    opt.Scheduling.IgnoreDuplicates = true;
    opt.Scheduling.OverWriteExistingData = true;
});

builder.Services.AddQuartz(configurator =>
{
    configurator.AddJob<ChatEngineJob>(opt => opt.WithIdentity(nameof(ChatEngineJob)));
    configurator.AddTrigger(opt => opt
        .ForJob(nameof(ChatEngineJob))
        .WithIdentity($"{nameof(ChatEngineJob)}-trigger")
        .StartNow()
        .WithSimpleSchedule(b =>
        {
            b.WithIntervalInSeconds(30);
            b.RepeatForever();
        }));
});

builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ContextDb>();
    await context.Database.EnsureDeletedAsync();
    await context.Database.MigrateAsync();

    var cfg = scope.ServiceProvider.GetRequiredService<IOptions<PromptConfig>>();
    await DbInitializer.Run(context, cfg.Value);
}

app.UseSwagger();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(_ => true));

app.MapControllers();

app.Run();
