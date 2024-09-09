using System.Globalization;
using System.Text.Json;
using CoindeskApi.Models;
using CoindeskApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CoindeskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoindeskController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICurrencyRepository _currencyRepository;

    public CoindeskController(IHttpClientFactory httpClientFactory, ICurrencyRepository currencyRepository)
    {
        _httpClientFactory = httpClientFactory;
        _currencyRepository = currencyRepository;
    }

    [HttpGet("query")]
    public async Task<IActionResult> Query()
    {
        var httpClient = _httpClientFactory.CreateClient("coindesk");
        var httpResponseMessage = await httpClient.GetAsync(
            "v1/bpi/currentprice.json");
        
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return BadRequest();
        }

        await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
        var coindesk = await JsonSerializer.DeserializeAsync<BitcoinPriceIndex>(contentStream);
        
        var dateTime  = DateTime.ParseExact(coindesk.Time.Updated, "MMM d, yyyy HH:mm:ss 'UTC'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

        // TODO 排序, 反射
        var vo = new RateVo
        {
            UpdateTime = dateTime.ToString("yyyy/MM/dd HH:mm:ss"),
            Currencies = new List<RateVo.Currency>()
        };
        var currency1 = new RateVo.Currency
        {
            Code = "USD",
            Name = (await _currencyRepository.GetAsync("USD"))?.CurrencyName,
            Rate = coindesk.Bpi.USD.RateFloat
        };
        vo.Currencies.Add(currency1);
        
        var currency2 = new RateVo.Currency
        {
            Code = "GBP",
            Name = (await _currencyRepository.GetAsync("GBP"))?.CurrencyName,
            Rate = coindesk.Bpi.GBP.RateFloat
        };
        vo.Currencies.Add(currency2);
        
        var currency3 = new RateVo.Currency
        {
            Code = "EUR",
            Name = (await _currencyRepository.GetAsync("EUR"))?.CurrencyName,
            Rate = coindesk.Bpi.EUR.RateFloat
        };
        vo.Currencies.Add(currency3);
        
        return Ok(vo);
    }
}

public class RateVo
{
    public string UpdateTime { get; set; }

    public List<Currency> Currencies { get; set; }

    public class Currency
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public float Rate { get; set; }
    }
}