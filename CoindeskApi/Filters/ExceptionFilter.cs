using CoindeskApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoindeskApi.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        _logger.LogError(exception.ToString());

        var failResponse = ApiResponseVo<object>.CreateFailure(new List<string>() { exception.Message });
        context.ExceptionHandled = true;
        context.Result = new JsonResult(failResponse)
        {
            StatusCode = 200
        };
    }
}