using System.Text;

namespace CoindeskApi.Middlewares;

internal class ApiLogMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiLogMiddleware> _logger;

    public ApiLogMiddleware(RequestDelegate next, ILogger<ApiLogMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.ToString().Contains("swagger"))
        {
            await _next(context);
            return;
        }

        var replacedRequestBody = new MemoryStream();
        var originalRequestBody = context.Request.Body;

        await context.Request.Body.CopyToAsync(replacedRequestBody);
        replacedRequestBody.Seek(0, SeekOrigin.Begin);

        string requestContent;
        using (var requestReader = new StreamReader(replacedRequestBody, encoding: Encoding.UTF8, true, 1024, true))
        {
            requestContent = await requestReader.ReadToEndAsync();
        }

        replacedRequestBody.Seek(0, SeekOrigin.Begin);
        context.Request.Body = replacedRequestBody;

        _logger.LogInformation(
            $"RequestUrl: {context.Request.Path.ToString()}, QueryString: {context.Request.QueryString.ToString()}, RequestBody: {requestContent}");

        var originalResponseBody = context.Response.Body;

        using (var replacedResponseBody = new MemoryStream())
        {
            context.Response.Body = replacedResponseBody;
            
            // call the next delegate/middleware in the pipeline
            await _next(context);
            
            context.Request.Body = originalRequestBody;

            replacedResponseBody.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(replacedResponseBody))
            {
                var responseContent = await reader.ReadToEndAsync();
                _logger.LogInformation($"Request Url: {context.Request.Path.ToString()}, ResponseBody: {responseContent}");
                replacedResponseBody.Seek(0, SeekOrigin.Begin);

                await replacedResponseBody.CopyToAsync(originalResponseBody);
            }
        }
    }
}