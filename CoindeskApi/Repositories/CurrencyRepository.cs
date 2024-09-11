using CoindeskApi.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace CoindeskApi.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    protected readonly CurrencyContext _context;

    public CurrencyRepository(CurrencyContext context)
    {
        _context = context;
    }

    public void Add(CurrencyEntity currency)
    {
        _context.Currencies.Add(currency);
    }

    public async Task<CurrencyEntity?> GetAsync(string currencyCode)
    {
        return await _context.Currencies.FirstOrDefaultAsync(x => x.Code == currencyCode);
    }

    public async Task<List<CurrencyEntity>> GetAsync()
    {
        return await _context.Currencies.ToListAsync();
    }

    public void Remove(CurrencyEntity currency)
    {
        _context.Currencies.Remove(currency);
    }

    public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}