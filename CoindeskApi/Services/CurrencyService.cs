using System.Globalization;
using CoindeskApi.Models;
using CoindeskApi.Models.Domains;
using CoindeskApi.Repositories;
using CoindeskApi.ViewModels;

namespace CoindeskApi.Services;

public class CurrencyService : ICurrencyService
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly ILogger<CurrencyService> _logger;
    private readonly HttpClient _httpClient;
    private readonly ICoindeskApiService _coindeskApiService;

    public CurrencyService(IHttpClientFactory httpClientFactory, ICurrencyRepository currencyRepository, ILogger<CurrencyService> logger, ICoindeskApiService coindeskApiService)
    {
        _currencyRepository = currencyRepository;
        _logger = logger;
        _coindeskApiService = coindeskApiService;
        _httpClient = httpClientFactory.CreateClient("coindesk");
    }

    public async Task CreateAsync(CreateCurrencyDto createCurrencyDto)
    {
        var existCurrency = await _currencyRepository.GetAsync(createCurrencyDto.Code);
        if (existCurrency != null)
        {
            // TODO 錯誤處理
            throw new NotImplementedException();
        }
        
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

    public async Task<List<CurrencyVo>> ReadAsync()
    {
        var currencies = await _currencyRepository.GetAsync();
        var result = currencies.OrderBy(x => x.Code).Select(x => new CurrencyVo
        {
            Code = x.Code,
            CurrencyName = x.CurrencyName
        }).ToList();

        return result;
    }

    public async Task UpdateAsync(UpdateCurrencyDto updateCurrencyDto)
    {
        var currency = await _currencyRepository.GetAsync(updateCurrencyDto.Code);
        if (currency == null)
        {
            // TODO 錯誤處理
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
            // TODO 錯誤處理
            throw new NotImplementedException();
        }
        
        _currencyRepository.Remove(currency);
        await _currencyRepository.SaveEntitiesAsync();
    }

    public async Task<PriceVo> GetPricesAsync()
    {
        var bitcoinPriceIndex = await _coindeskApiService.GetBitcoinPriceIndexAsync();

        var dateTime  = DateTime.ParseExact(bitcoinPriceIndex.Time.Updated, "MMM d, yyyy HH:mm:ss 'UTC'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

        var bpiCurrencyInfo = new List<CurrencyInfo>() {bitcoinPriceIndex.Bpi.USD, bitcoinPriceIndex.Bpi.GBP, bitcoinPriceIndex.Bpi.EUR};
        var currencies = await _currencyRepository.GetAsync();
        currencies = currencies.Where(c => bpiCurrencyInfo.Any(bpi => bpi.Code == c.Code)).OrderBy(x => x.Code).ToList();

        var priceVo = new PriceVo()
        {
            UpdateTime = dateTime.ToString("yyyy/MM/dd HH:mm:ss"),
            Currencies = currencies.Select(c => new PriceVo.CurrencyRateVo()
            {
                Code = c.Code,
                Name = c.CurrencyName,
                Rate = bpiCurrencyInfo.FirstOrDefault(bpi => bpi.Code == c.Code)!.RateFloat
            }).ToList()
        };

        return priceVo;
    }
}