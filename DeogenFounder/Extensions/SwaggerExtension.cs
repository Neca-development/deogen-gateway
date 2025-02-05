using Microsoft.OpenApi.Models;

namespace DeogenFounder.Extensions;

public static class SwaggerExtension
{
    public static void AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Chatify API",
                Contact = new OpenApiContact
                {
                    Name = "Danya Skablov",
                    Email = "danilaskablov@gmail.com",
                }
            });
        });
    }
    
    public static void UseSwagger(this WebApplication app)
    {
        app.UseSwagger(c =>
        {
            c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "V1");
            c.RoutePrefix = "api/swagger";
        });
    }
}
