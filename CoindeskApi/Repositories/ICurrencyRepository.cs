using CoindeskApi.Models.Domains;

namespace CoindeskApi.Repositories;

public interface ICurrencyRepository
{
    public void Add(CurrencyEntity currency);
    
    public Task<CurrencyEntity?> GetAsync(string currencyCode);
    
    public Task<List<CurrencyEntity>> GetAsync();
    
    public List<CurrencyEntity> Get();
    
    public void Remove(CurrencyEntity currency);
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

    int SaveChanges(CancellationToken cancellationToken = default(CancellationToken));
}