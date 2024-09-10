using CoindeskApi.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CoindeskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    /// <summary>
    /// 測試 Get 丟出例外
    /// </summary>
    /// <param name="name"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    [HttpGet("exception_by_get")]
    [SwaggerOperation(Tags = new[] { "測試用" })]
    public IActionResult ExceptionByGet([FromQuery]string name, int count)
    {
        var zero = 0;
        var divideByZero = 100 / zero;
        
        return Ok($"hello world!! {name}, {count}");
    }
    
    /// <summary>
    /// 測試 Post 丟出例外
    /// </summary>
    /// <param name="createCurrencyDto"></param>
    /// <returns></returns>
    [HttpPost("exception_by_post")]
    [SwaggerOperation(Tags = new[] { "測試用" })]
    public IActionResult ExceptionByPost([FromBody] CreateCurrencyDto createCurrencyDto)
    {
        var zero = 0;
        var divideByZero = 100 / zero;
        
        return Ok($"hello world!!");
    }
}