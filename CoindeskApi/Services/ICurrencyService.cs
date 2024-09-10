using CoindeskApi.Models;
using CoindeskApi.Models.Domains;
using CoindeskApi.ViewModels;

namespace CoindeskApi.Services;

public interface ICurrencyService
{
    Task CreateAsync(CreateCurrencyDto createCurrencyDto);
    Task<List<CurrencyVo>> ReadAsync();
    Task UpdateAsync(UpdateCurrencyDto updateCurrencyDto);
    Task DeleteAsync(DeleteCurrencyDto deleteCurrencyDto);
    Task<PriceVo> GetPricesAsync();
}