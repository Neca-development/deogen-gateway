using System.Net;
using DeogenFounder.Common.DTO;
using DeogenFounder.Common.Exceptions;

namespace DeogenFounder.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception err)
        {
            var message = new List<string>();
            
            if (err is BaseException httpException)
            {
                context.Response.StatusCode = httpException.StatusCode;
                message = httpException.Errors;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                message.Add("Something went wrong");
                logger.LogError("Something went wrong: {Err}", err);
            }
            
            var response = FormatResponse(message.ToArray(), context.Response.StatusCode);
            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private static ApiResponse FormatResponse(string[] errors, int status)
    {
        return new ApiResponse
        {
            Errors = errors,
            Status = status
        };
    }
}
