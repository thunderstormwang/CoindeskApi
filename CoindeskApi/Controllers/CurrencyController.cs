using CoindeskApi.Models.Domains;
using CoindeskApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CoindeskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : Controller
{
    private readonly ICurrencyRepository _currencyRepository;

    public CurrencyController(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CurrencyDto currencyDto)
    {
        var currency = new CurrencyEntity
        {
            Code = currencyDto.Code,
            Lang = "zh-TW",
            CurrencyName = currencyDto.CurrencyName
        };
        _currencyRepository.Add(currency);
        await _currencyRepository.SaveEntitiesAsync();
        
        return Ok("成功");
    }
    
    [HttpGet("read")]
    public async Task<IActionResult> Read()
    {
        var result = await _currencyRepository.GetAsync();
        
        return Ok(result);
    }
    
    [HttpPost("update")]
    public async Task<IActionResult> Update(CurrencyDto currencyDto)
    {
        var currency = await _currencyRepository.GetAsync(currencyDto.Code);
        if (currency == null)
        {
            return Ok("找不到資料");
        }
        
        currency.CurrencyName = currencyDto.CurrencyName;
        await _currencyRepository.SaveEntitiesAsync();
        
        return Ok("成功");
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(CurrencyDto currencyDto)
    {
        var currency = await _currencyRepository.GetAsync(currencyDto.Code);
        if (currency == null)
        {
            return Ok("找不到資料");
        }
        
        _currencyRepository.Remove(currency);
        await _currencyRepository.SaveEntitiesAsync();
        
        return Ok("成功");
    }
}