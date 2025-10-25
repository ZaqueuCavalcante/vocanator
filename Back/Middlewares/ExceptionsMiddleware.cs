using System.Text.Json;

namespace Vocanator.Back.Middlewares;

public class ExceptionsMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var result = JsonSerializer.Serialize(new { Message = "Internal Server Error" });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        return context.Response.WriteAsync(result);
    }
}
