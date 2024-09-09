using CoindeskApi.Models;
using CoindeskApi.Models.Domains;
using CoindeskApi.ViewModels;

namespace CoindeskApi.Services;

public interface ICurrencyService
{
    Task CreateAsync(CreateCurrencyDto createCurrencyDto);
    Task<List<CurrencyEntity>> ReadAsync();
    Task UpdateAsync(UpdateCurrencyDto updateCurrencyDto);
    Task DeleteAsync(DeleteCurrencyDto deleteCurrencyDto);
    Task<PriceVo> GetPricesAsync();
}