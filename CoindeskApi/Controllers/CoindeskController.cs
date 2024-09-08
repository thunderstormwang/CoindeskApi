using System.Text.Json;
using CoindeskApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoindeskApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoindeskController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CoindeskController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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
        return Ok(coindesk);
    }
}