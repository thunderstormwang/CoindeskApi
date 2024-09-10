using CoindeskApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoindeskApi.Filters;

public class ValidateModelFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errorMessages = context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            var failResponse = ApiResponseVo<object>.CreateFailure(errorMessages.ToList());
            context.Result = new OkObjectResult(failResponse);
            
            return;
        }
        
        await next();
    }
}