using CoindeskApi.Filters;
using CoindeskApi.Models;
using CoindeskApi.Models.Domains;
using CoindeskApi.Services;
using CoindeskApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateCurrencyDto createCurrencyDto)
    {
        await _currencyService.CreateAsync(createCurrencyDto);
        
        return Ok(ApiResponseVo<bool>.CreateSuccess(true));
    }
    
    [HttpGet("read")]
    public async Task<IActionResult> Read()
    {
        var result = await _currencyService.ReadAsync();
        
        return Ok(ApiResponseVo<List<CurrencyVo>>.CreateSuccess(result));
    }
    
    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateCurrencyDto createCurrencyDto)
    {
        await _currencyService.UpdateAsync(createCurrencyDto);
        
        return Ok(ApiResponseVo<bool>.CreateSuccess(true));
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(DeleteCurrencyDto createCurrencyDto)
    {
        await _currencyService.DeleteAsync(createCurrencyDto);
        
        return Ok(ApiResponseVo<bool>.CreateSuccess(true));
    }
}