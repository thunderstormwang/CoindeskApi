using CoindeskApi.Models;
using CoindeskApi.Services;
using CoindeskApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CoindeskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : Controller
{
    private readonly ICurrencyService _currencyService;

    public CurrencyController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="createCurrencyDto"></param>
    /// <returns></returns>
    [HttpPost("create")]
    [SwaggerOperation(Tags = new[] { "幣別維護" })]
    [ProducesResponseType(typeof(ApiResponseVo<bool>), 200)]
    public async Task<IActionResult> Create([FromBody] CreateCurrencyDto createCurrencyDto)
    {
        await _currencyService.CreateAsync(createCurrencyDto);
        
        return Ok(ApiResponseVo<bool>.CreateSuccess(true));
    }
    
    /// <summary>
    /// 取得
    /// </summary>
    /// <returns></returns>
    [HttpGet("read")]
    [SwaggerOperation(Tags = new[] { "幣別維護" })]
    [ProducesResponseType(typeof(ApiResponseVo<List<CurrencyVo>>), 200)]
    public async Task<IActionResult> Read()
    {
        var result = await _currencyService.ReadAsync();
        
        return Ok(ApiResponseVo<List<CurrencyVo>>.CreateSuccess(result));
    }
    
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="createCurrencyDto"></param>
    /// <returns></returns>
    [HttpPost("update")]
    [SwaggerOperation(Tags = new[] { "幣別維護" })]
    [ProducesResponseType(typeof(ApiResponseVo<bool>), 200)]
    public async Task<IActionResult> Update(UpdateCurrencyDto createCurrencyDto)
    {
        await _currencyService.UpdateAsync(createCurrencyDto);
        
        return Ok(ApiResponseVo<bool>.CreateSuccess(true));
    }
    
    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="createCurrencyDto"></param>
    /// <returns></returns>
    [HttpPost("delete")]
    [SwaggerOperation(Tags = new[] { "幣別維護" })]
    [ProducesResponseType(typeof(ApiResponseVo<bool>), 200)]
    public async Task<IActionResult> Delete(DeleteCurrencyDto createCurrencyDto)
    {
        await _currencyService.DeleteAsync(createCurrencyDto);
        
        return Ok(ApiResponseVo<bool>.CreateSuccess(true));
    }
}