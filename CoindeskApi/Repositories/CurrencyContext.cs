using CoindeskApi.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace CoindeskApi.Repositories;

public class CurrencyContext : DbContext
{
    private readonly MyCommandInterceptor _myCommandInterceptor;
    private readonly ILogger<CurrencyContext> _logger;

    public CurrencyContext(DbContextOptions<CurrencyContext> options, MyCommandInterceptor myCommandInterceptor,
        ILogger<CurrencyContext> logger)
        : base(options)
    {
        _myCommandInterceptor = myCommandInterceptor;
        _logger = logger;
    }

    public DbSet<CurrencyEntity> Currencies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .LogTo(message => _logger.LogInformation($"Simple Logging: {message}"),
                new[] { DbLoggerCategory.Database.Name }, LogLevel.Information)
            .AddInterceptors(_myCommandInterceptor);
    }
}