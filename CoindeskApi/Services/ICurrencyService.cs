using CoindeskApi.Models;
using CoindeskApi.Models.Domains;
using CoindeskApi.ViewModels;

namespace CoindeskApi.Services;

public interface ICurrencyService
{
    Task CreateAsync(CurrencyDto currencyDto);
    Task<List<CurrencyEntity>> ReadAsync();
    Task UpdateAsync(CurrencyDto currencyDto);
    Task DeleteAsync(CurrencyDto currencyDto);
    Task<PriceVo> GetPricesAsync();
}