using CoindeskApi.Models;
using CoindeskApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CoindeskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ICurrencyRepository _currencyRepository;

    public TestController(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

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
    
    /// <summary>
    /// 測試同步取得
    /// </summary>
    /// <returns></returns>
    [HttpGet("test_non_async")]
    [SwaggerOperation(Tags = new[] { "測試用" })]
    public IActionResult TestNonAsync()
    {
        var result = _currencyRepository.Get();

        return Ok(result);
    }
    
    /// <summary>
    /// 測試非同步取得
    /// </summary>
    /// <returns></returns>
    [HttpGet("test_async")]
    [SwaggerOperation(Tags = new[] { "測試用" })]
    public async Task<IActionResult> TestAsync()
    {
        var result = await _currencyRepository.GetAsync();

        return Ok(result);
    }
}