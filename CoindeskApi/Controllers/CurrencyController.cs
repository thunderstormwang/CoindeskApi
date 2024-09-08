using CoindeskApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoindeskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : Controller
{
    private readonly CurrencyContext _context;

    public CurrencyController(CurrencyContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CurrencyDto currencyDto)
    {
        var currency = new Currency
        {
            Code = currencyDto.Code,
            Lang = "zh-TW",
            CurrencyName = currencyDto.CurrencyName
        };
        _context.Currencies.Add(currency);
        await _context.SaveChangesAsync();
        
        return Ok("成功");
    }
    
    [HttpGet("read")]
    public async Task<IActionResult> Read()
    {
        var result = _context.Currencies.ToList();
        
        return Ok(result);
    }
    
    [HttpPost("update")]
    public async Task<IActionResult> Update(CurrencyDto currencyDto)
    {
        var currency = _context.Currencies.FirstOrDefault(x => x.Code == currencyDto.Code);
        if (currency == null)
        {
            return Ok("找不到資料");
        }
        
        currency.CurrencyName = currencyDto.CurrencyName;
        await _context.SaveChangesAsync();
        
        return Ok("成功");
    }
    
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(CurrencyDto currencyDto)
    {
        var currency = _context.Currencies.FirstOrDefault(x => x.Code == currencyDto.Code);
        if (currency == null)
        {
            return Ok("找不到資料");
        }
        
        _context.Currencies.Remove(currency);
        await _context.SaveChangesAsync();
        
        return Ok("成功");
    }
}