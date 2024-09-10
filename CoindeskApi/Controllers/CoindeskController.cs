using CoindeskApi.Services;
using CoindeskApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

    /// <summary>
    /// 取得報價
    /// </summary>
    /// <returns></returns>
    [HttpGet("get_price")]
    [SwaggerOperation(Tags = new[] { "報價" })]
    [ProducesResponseType(typeof(ApiResponseVo<PriceVo>), 200)]
    public async Task<IActionResult> GetPrice()
    {
        var result = await _currencyService.GetPricesAsync();
        
        return Ok(ApiResponseVo<PriceVo>.CreateSuccess(result));
    }
}