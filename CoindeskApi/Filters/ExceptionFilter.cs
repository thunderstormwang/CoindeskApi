using Microsoft.AspNetCore.Mvc.Filters;

namespace CoindeskApi.Filters;

public class ExceptionFilter :IAsyncExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var exception = context.Exception;
        _logger.LogError(exception.ToString());
        
        context.ExceptionHandled = true;
        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.StatusCode = 200;
        await context.HttpContext.Response.WriteAsync(exception.Message);
    }
}