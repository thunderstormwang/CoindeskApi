using CoindeskApi.Controllers;
using CoindeskApi.Models;
using CoindeskApi.Models.Domains;
using CoindeskApi.Repositories;
using CoindeskApi.Services;
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

        builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        builder.Services.AddScoped<ICurrencyService, CurrencyService>();
        
        builder.Services.AddHttpClient("coindesk", httpClient =>
        {
            httpClient.BaseAddress = new Uri(builder.Configuration.GetSection("Coindesk:Url").Value);
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
        var currency1 = new CurrencyEntity
        {
            Code = "USD",
            Lang = "zh-TW",
            CurrencyName = "美金",
            CreateTime = DateTime.Now
        };
        currencyContext.Currencies.Add(currency1);
        
        var currency2 = new CurrencyEntity
        {
            Code = "GBP",
            Lang = "zh-TW",
            CurrencyName = "英鎊",
            CreateTime = DateTime.Now
        };
        currencyContext.Currencies.Add(currency2);
        
        var currency3 = new CurrencyEntity
        {
            Code = "EUR",
            Lang = "zh-TW",
            CurrencyName = "歐元",
            CreateTime = DateTime.Now
        };
        currencyContext.Currencies.Add(currency3);
        currencyContext.SaveChanges();
    }
}