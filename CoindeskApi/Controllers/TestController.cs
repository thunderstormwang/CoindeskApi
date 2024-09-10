using CoindeskApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoindeskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("exception_by_get")]
    public IActionResult ExceptionByGet([FromQuery]string name, int count)
    {
        var zero = 0;
        var divideByZero = 100 / zero;
        
        return Ok($"hello world!! {name}, {count}");
    }
    
    [HttpPost("exception_by_post")]
    public IActionResult ExceptionByPost([FromBody] CreateCurrencyDto createCurrencyDto)
    {
        var zero = 0;
        var divideByZero = 100 / zero;
        
        return Ok($"hello world!!");
    }
}