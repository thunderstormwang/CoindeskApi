using System.Text.Json;
using CoindeskApi.Models;

namespace CoindeskApi.Services;

public class CoindeskApiService : ICoindeskApiService
{
    private readonly ILogger<CoindeskApiService> _logger;
    private readonly HttpClient _httpClient;

    public CoindeskApiService(ILogger<CoindeskApiService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("coindesk");
    }

    public async Task<BitcoinPriceIndex?> GetBitcoinPriceIndexAsync()
    {
        var url = "v1/bpi/currentprice.json";
        var responseStr = await GetStringByGetAsync(url);
        var bitcoinPriceIndex = JsonSerializer.Deserialize<BitcoinPriceIndex>(responseStr);

        return bitcoinPriceIndex;
    }

    private async Task<string> GetStringByGetAsync(string url)
    {
        _logger.LogInformation($"GetAsync Request path: {_httpClient.BaseAddress}{url}");
        var httpResponseMessage = await _httpClient.GetAsync(url);
        
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"呼叫外部 api 錯誤: {httpResponseMessage.StatusCode}");
        }

        var responseStr = await httpResponseMessage.Content.ReadAsStringAsync();
        _logger.LogInformation($"GetAsync Response path: {_httpClient.BaseAddress}{url}, response: {responseStr}");
        
        return responseStr;
    }
}