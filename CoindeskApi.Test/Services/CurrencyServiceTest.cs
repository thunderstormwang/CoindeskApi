using CoindeskApi.Models;
using CoindeskApi.Models.Domains;
using CoindeskApi.Repositories;
using CoindeskApi.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace CoindeskApi.Test.Services;

[Collection("Sequential")]
public class CurrencyServiceTest
{
    private IConfiguration _configuration;
    private readonly ServiceProvider _serviceProvider;

    public CurrencyServiceTest()
    {
        var serviceCollection = new ServiceCollection();
        LoadConfig(serviceCollection);
        RegisterLogger(serviceCollection);
        RegisterService(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [Fact(DisplayName = "新增 - 成功")]
    public async Task CreateAsync_Success()
    {
        await AddInitialData();
        
        var targetService = _serviceProvider.GetRequiredService<ICurrencyService>();

        var repo = _serviceProvider.GetRequiredService<ICurrencyRepository>();

        var request = new CreateCurrencyDto()
        {
            Code = "TWD",
            CurrencyName = "新台幣"
        };

        await targetService.CreateAsync(request);

        var actualCurrency = await repo.GetAsync(request.Code);

        actualCurrency.Should().NotBeNull();
        actualCurrency.Code.Should().Be(request.Code);
        actualCurrency.CurrencyName.Should().Be(request.CurrencyName);
    }

    [Fact(DisplayName = "新增 - 資料已存在")]
    public async Task CreateAsync_DataExist()
    {
        await AddInitialData();
        
        var targetService = _serviceProvider.GetRequiredService<ICurrencyService>();

        var request = new CreateCurrencyDto()
        {
            Code = "USD",
            CurrencyName = "美金"
        };
        
        var func = async () => await targetService.CreateAsync(request);
        await func.Should().ThrowAsync<Exception>().Where(e => e.Message == "資料已存在");
    }

    private void LoadConfig(IServiceCollection services)
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.test.json", false, false);
        _configuration = configBuilder.Build();
        services.AddSingleton(_configuration);
    }

    private void RegisterLogger(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient(p => new Mock<ILogger<CurrencyService>>().Object);
        serviceCollection.AddTransient(p => new Mock<ILogger<CoindeskApiService>>().Object);
    }

    private void RegisterService(ServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<CurrencyContext>(options => { options.UseInMemoryDatabase("Currency"); });
        serviceCollection.AddScoped<ICurrencyRepository, CurrencyRepository>();
        serviceCollection.AddScoped<ICoindeskApiService, CoindeskApiService>();
        serviceCollection.AddScoped<ICurrencyService, CurrencyService>();

        serviceCollection.AddHttpClient("coindesk", c =>
        {
            c.BaseAddress = new Uri(_configuration.GetSection("Coindesk:Url").Value);
        });
    }

    private async Task AddInitialData()
    {
        var repo = _serviceProvider.GetRequiredService<ICurrencyRepository>();
        var usd = new CurrencyEntity
        {
            Code = "USD",
            Lang = "zh-TW",
            CurrencyName = "美金",
            CreateTime = DateTime.Now
        };
        repo.Add(usd);

        var gbp = new CurrencyEntity
        {
            Code = "GBP",
            Lang = "zh-TW",
            CurrencyName = "英鎊",
            CreateTime = DateTime.Now
        };
        repo.Add(gbp);

        var eur = new CurrencyEntity
        {
            Code = "EUR",
            Lang = "zh-TW",
            CurrencyName = "歐元",
            CreateTime = DateTime.Now
        };
        repo.Add(eur);
        await repo.SaveEntitiesAsync();
    }
}