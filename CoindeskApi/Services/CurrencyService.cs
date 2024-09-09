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

    public async Task CreateAsync(CurrencyDto currencyDto)
    {
        var currency = new CurrencyEntity
        {
            Code = currencyDto.Code,
            Lang = "zh-TW",
            CurrencyName = currencyDto.CurrencyName
        };
        _currencyRepository.Add(currency);
        
        await _currencyRepository.SaveEntitiesAsync();
    }

    public async Task<List<CurrencyEntity>> ReadAsync()
    {
        var result = await _currencyRepository.GetAsync();

        return result;
    }

    public async Task UpdateAsync(CurrencyDto currencyDto)
    {
        var currency = await _currencyRepository.GetAsync(currencyDto.Code);
        if (currency == null)
        {
            throw new NotImplementedException();
        }
        
        currency.CurrencyName = currencyDto.CurrencyName;
        await _currencyRepository.SaveEntitiesAsync();
    }

    public async Task DeleteAsync(CurrencyDto currencyDto)
    {
        var currency = await _currencyRepository.GetAsync(currencyDto.Code);
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
        var httpResponseMessage = await httpClient.GetAsync(
            "v1/bpi/currentprice.json");
        
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new NotImplementedException();
        }

        await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
        var coindesk = await JsonSerializer.DeserializeAsync<BitcoinPriceIndex>(contentStream);
        
        var dateTime  = DateTime.ParseExact(coindesk.Time.Updated, "MMM d, yyyy HH:mm:ss 'UTC'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

        // TODO 排序, 反射
        var vo = new PriceVo
        {
            UpdateTime = dateTime.ToString("yyyy/MM/dd HH:mm:ss"),
            Currencies = new List<PriceVo.Currency>()
        };
        var currency1 = new PriceVo.Currency
        {
            Code = "USD",
            Name = (await _currencyRepository.GetAsync("USD"))?.CurrencyName,
            Rate = coindesk.Bpi.USD.RateFloat
        };
        vo.Currencies.Add(currency1);
        
        var currency2 = new PriceVo.Currency
        {
            Code = "GBP",
            Name = (await _currencyRepository.GetAsync("GBP"))?.CurrencyName,
            Rate = coindesk.Bpi.GBP.RateFloat
        };
        vo.Currencies.Add(currency2);
        
        var currency3 = new PriceVo.Currency
        {
            Code = "EUR",
            Name = (await _currencyRepository.GetAsync("EUR"))?.CurrencyName,
            Rate = coindesk.Bpi.EUR.RateFloat
        };
        vo.Currencies.Add(currency3);

        return vo;
    }
}