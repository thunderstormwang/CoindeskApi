using CoindeskApi.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace CoindeskApi.Repositories;

public class CurrencyContext : DbContext
{
    public CurrencyContext(DbContextOptions<CurrencyContext> options)
        : base(options)
    {
    }

    public DbSet<CurrencyEntity> Currencies { get; set; }
}