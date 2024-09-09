using CoindeskApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoindeskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoindeskController : ControllerBase
{
    private readonly ICurrencyService _currencyService;

    public CoindeskController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpGet("get_price")]
    public async Task<IActionResult> GetPrice()
    {
        var vo = await _currencyService.GetPricesAsync();
        
        return Ok(vo);
    }
}