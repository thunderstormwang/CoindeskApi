using CoindeskApi.Models;

namespace CoindeskApi.Services;

public interface ICoindeskApiService
{
    Task<BitcoinPriceIndex?> GetBitcoinPriceIndexAsync();
}