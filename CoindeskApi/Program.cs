using CoindeskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CoindeskApi;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContext<CurrencyContext>(opt =>
            opt.UseInMemoryDatabase("Currency"), contextLifetime: ServiceLifetime.Singleton);
        // Add services to the container.
        // // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddHttpClient("coindesk", httpClient =>
        {
            // TODO config
            httpClient.BaseAddress = new Uri("https://api.coindesk.com/");
        });

        var app = builder.Build();

        InitialData(app);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }

    private static void InitialData(WebApplication app)
    {
        var currencyContext = app.Services.GetRequiredService<CurrencyContext>();
        var currency1 = new Currency
        {
            Code = "USD",
            Lang = "zh-TW",
            CurrencyName = "美金"
        };
        currencyContext.Currencies.Add(currency1);
        
        var currency2 = new Currency
        {
            Code = "GBP",
            Lang = "zh-TW",
            CurrencyName = "英鎊"
        };
        currencyContext.Currencies.Add(currency2);
        
        var currency3 = new Currency
        {
            Code = "EUR",
            Lang = "zh-TW",
            CurrencyName = "歐元"
        };
        currencyContext.Currencies.Add(currency3);
        currencyContext.SaveChanges();
    }
}