using System.Text.Json.Serialization;
using DeogenFounder.Common.Configs;
using Microsoft.AspNetCore.Diagnostics;
using DeogenFounder.Extensions;
using DeogenFounder.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ContextDb>();
    await context.Database.MigrateAsync();
    // await context.Database.EnsureDeletedAsync();

    var cfg = scope.ServiceProvider.GetRequiredService<IOptions<PromptConfig>>();
    await DbInitializer.Run(context, cfg.Value);
}

app.UseSwagger();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(_ => true));

app.MapControllers();

app.Run();
