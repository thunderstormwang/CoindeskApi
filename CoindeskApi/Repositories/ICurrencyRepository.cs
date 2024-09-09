using CoindeskApi.Models.Domains;

namespace CoindeskApi.Repositories;

public interface ICurrencyRepository
{
    public void Add(CurrencyEntity currency);
    
    public Task<CurrencyEntity?> GetAsync(string currencyCode);
    
    public Task<List<CurrencyEntity>> GetAsync();
    
    public void Remove(CurrencyEntity currency);
    
    Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
}