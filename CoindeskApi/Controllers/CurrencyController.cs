using CoindeskApi.Models;
using CoindeskApi.Services;
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
        
        return Ok("成功");
    }
    
    [HttpGet("read")]
    public async Task<IActionResult> Read()
    {
        var result = await _currencyService.ReadAsync();
        
        return Ok(result);
    }
    
    [HttpPost("update")]
    public async Task<IActionResult> Update(UpdateCurrencyDto createCurrencyDto)
    {
        await _currencyService.UpdateAsync(createCurrencyDto);
        
        return Ok("成功");
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(DeleteCurrencyDto createCurrencyDto)
    {
        await _currencyService.DeleteAsync(createCurrencyDto);
        
        return Ok("成功");
    }
}