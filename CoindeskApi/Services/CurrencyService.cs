using System.Globalization;
using System.Text.Json;
using CoindeskApi.Models;
using CoindeskApi.Models.Domains;
using CoindeskApi.Repositories;
using CoindeskApi.ViewModels;

namespace CoindeskApi.Services;

public class CurrencyService : ICurrencyService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICurrencyRepository _currencyRepository;

    public CurrencyService(IHttpClientFactory httpClientFactory, ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
        _httpClientFactory = httpClientFactory;
    }

    public async Task CreateAsync(CreateCurrencyDto createCurrencyDto)
    {
        var currency = new CurrencyEntity
        {
            Code = createCurrencyDto.Code,
            Lang = "zh-TW",
            CurrencyName = createCurrencyDto.CurrencyName,
            CreateTime = DateTime.Now
        };
        _currencyRepository.Add(currency);
        
        await _currencyRepository.SaveEntitiesAsync();
    }

    public async Task<List<CurrencyEntity>> ReadAsync()
    {
        var result = await _currencyRepository.GetAsync();
        result = result.OrderBy(x => x.Code).ToList();

        return result;
    }

    public async Task UpdateAsync(UpdateCurrencyDto updateCurrencyDto)
    {
        var currency = await _currencyRepository.GetAsync(updateCurrencyDto.Code);
        if (currency == null)
        {
            throw new NotImplementedException();
        }
        
        currency.SetCurrencyName(updateCurrencyDto.CurrencyName);
        await _currencyRepository.SaveEntitiesAsync();
    }

    public async Task DeleteAsync(DeleteCurrencyDto deleteCurrencyDto)
    {
        var currency = await _currencyRepository.GetAsync(deleteCurrencyDto.Code);
        if (currency == null)
        {
            throw new NotImplementedException();
        }
        
        _currencyRepository.Remove(currency);
        await _currencyRepository.SaveEntitiesAsync();
    }

    public async Task<PriceVo> GetPricesAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("coindesk");
        var httpResponseMessage = await httpClient.GetAsync("v1/bpi/currentprice.json");
        
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new NotImplementedException();
        }

        await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
        var coindesk = await JsonSerializer.DeserializeAsync<BitcoinPriceIndex>(contentStream);
        
        var dateTime  = DateTime.ParseExact(coindesk.Time.Updated, "MMM d, yyyy HH:mm:ss 'UTC'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

        var bpiCurrencyInfo = new List<CurrencyInfo>() {coindesk.Bpi.USD, coindesk.Bpi.GBP, coindesk.Bpi.EUR};
        var currencies = await _currencyRepository.GetAsync();
        currencies = currencies.Where(c => bpiCurrencyInfo.Any(bpi => bpi.Code == c.Code)).OrderBy(x => x.Code).ToList();

        var priceVo = new PriceVo()
        {
            UpdateTime = dateTime.ToString("yyyy/MM/dd HH:mm:ss"),
            Currencies = currencies.Select(c => new PriceVo.Currency()
            {
                Code = c.Code,
                Name = c.CurrencyName,
                Rate = bpiCurrencyInfo.FirstOrDefault(bpi => bpi.Code == c.Code)!.RateFloat
            }).ToList()
        };

        return priceVo;
    }
}