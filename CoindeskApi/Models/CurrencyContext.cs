using CoindeskApi.Controllers;
using Microsoft.EntityFrameworkCore;

namespace CoindeskApi.Models;

public class CurrencyContext : DbContext
{
    public CurrencyContext(DbContextOptions<CurrencyContext> options)
        : base(options)
    {
    }

    public DbSet<Currency> Currencies { get; set; }
}