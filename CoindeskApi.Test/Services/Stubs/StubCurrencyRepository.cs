using CoindeskApi.Repositories;

namespace CoindeskApi.Test.Services.Stubs;

internal class StubCurrencyRepository : CurrencyRepository
{
    public StubCurrencyRepository(CurrencyContext context) : base(context)
    {
    }

    public async Task RemoveAllAsync()
    {
        var entities = _context.Currencies.ToList();

        foreach (var entity in entities)
        {
            _context.Currencies.Remove(entity);
        }

        await _context.SaveChangesAsync();
    }
}