using CoindeskApi.Models;
using CoindeskApi.Models.Domains;
using CoindeskApi.Repositories;
using CoindeskApi.Services;
using CoindeskApi.Test.Services.Stubs;
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
    
    protected Mock<ICoindeskApiService> _mockCoindeskApiService;

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

        var expected = new CurrencyEntity()
        {
            Code = request.Code,
            CurrencyName = request.CurrencyName
        };

        var actual = await repo.GetAsync(expected.Code);

        actual.Should().NotBeNull();
        actual.Code.Should().Be(expected.Code);
        actual.CurrencyName.Should().Be(expected.CurrencyName);
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

    [Fact(DisplayName = "修改 - 成功")]
    public async Task UpdateAsync_Success()
    {
        await AddInitialData();

        var targetService = _serviceProvider.GetRequiredService<ICurrencyService>();
        var repo = _serviceProvider.GetRequiredService<ICurrencyRepository>();

        var request = new UpdateCurrencyDto()
        {
            Code = "USD",
            CurrencyName = "美元好棒"
        };

        await targetService.UpdateAsync(request);

        var expected = new CurrencyEntity()
        {
            Code = request.Code,
            CurrencyName = request.CurrencyName
        };

        var actual = await repo.GetAsync(expected.Code);

        actual.Should().NotBeNull();
        actual.Code.Should().Be(expected.Code);
        actual.CurrencyName.Should().Be(expected.CurrencyName);
    }

    [Fact(DisplayName = "修改 - 資料不存在")]
    public async Task UpdateAsync_DataNotExist()
    {
        await AddInitialData();

        var targetService = _serviceProvider.GetRequiredService<ICurrencyService>();

        var request = new UpdateCurrencyDto()
        {
            Code = "TWD",
            CurrencyName = "新台幣"
        };

        var func = async () => await targetService.UpdateAsync(request);
        await func.Should().ThrowAsync<Exception>().Where(e => e.Message == "資料不存在");
    }

    [Fact(DisplayName = "刪除 - 成功")]
    public async Task DeleteAsync_Success()
    {
        await AddInitialData();

        var targetService = _serviceProvider.GetRequiredService<ICurrencyService>();
        var repo = _serviceProvider.GetRequiredService<ICurrencyRepository>();

        var request = new DeleteCurrencyDto()
        {
            Code = "USD"
        };

        await targetService.DeleteAsync(request);

        var actual = await repo.GetAsync(request.Code);

        actual.Should().BeNull();
    }
    
    [Fact(DisplayName = "刪除 - 資料不存在")]
    public async Task DeleteAsync_DataNotExist()
    {
        await AddInitialData();

        var targetService = _serviceProvider.GetRequiredService<ICurrencyService>();

        var request = new DeleteCurrencyDto()
        {
            Code = "TWD"
        };

        var func = async () => await targetService.DeleteAsync(request);
        await func.Should().ThrowAsync<Exception>().Where(e => e.Message == "資料不存在");
    }
    
    [Fact(DisplayName = "讀取 - 成功")]
    public async Task ReadAsync_Success()
    {
        await AddInitialData();

        var targetService = _serviceProvider.GetRequiredService<ICurrencyService>();
        
        var actual = await targetService.ReadAsync();

        actual.Should().NotBeNull();
        actual.Count.Should().Be(3);
        actual[0].Code.Should().Be("EUR");
        actual[0].CurrencyName.Should().Be("歐元");
        actual[1].Code.Should().Be("GBP");
        actual[1].CurrencyName.Should().Be("英鎊");
        actual[2].Code.Should().Be("USD");
        actual[2].CurrencyName.Should().Be("美金");
    }
    
    [Fact(DisplayName = "GetPricesAsync - 成功")]
    public async Task GetPricesAsync_Success()
    {
        await AddInitialData();

        var mockGetBitcoinPriceIndexResponse = new BitcoinPriceIndex
        {
            Time = new TimeInfo
            {
                Updated = "Sep 11, 2024 09:14:55 UTC"
            },
            Bpi = new BpiInfo
            {
                USD = new CurrencyInfo
                {
                    Code = "USD",
                    RateFloat = 30.0f
                },
                GBP = new CurrencyInfo
                {
                    Code = "GBP",
                    RateFloat = 20.0f
                },
                EUR = new CurrencyInfo
                {
                    Code = "EUR",
                    RateFloat = 10.0f
                }
            }
        };
        _mockCoindeskApiService.Setup(m => m.GetBitcoinPriceIndexAsync()).ReturnsAsync(mockGetBitcoinPriceIndexResponse);

        var targetService = _serviceProvider.GetRequiredService<ICurrencyService>();
        
        var actual = await targetService.GetPricesAsync();

        actual.Should().NotBeNull();
        actual.Currencies.Should().NotBeNull();
        actual.Currencies.Count.Should().Be(3);
        actual.Currencies[0].Code.Should().Be("EUR");
        actual.Currencies[0].Rate.Should().Be(10.0f);
        actual.Currencies[1].Code.Should().Be("GBP");
        actual.Currencies[1].Rate.Should().Be(20.0f);
        actual.Currencies[2].Code.Should().Be("USD");
        actual.Currencies[2].Rate.Should().Be(30.0f);
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
        serviceCollection.AddScoped<ICurrencyRepository, StubCurrencyRepository>();
        serviceCollection.AddScoped<ICurrencyService, CurrencyService>();

        _mockCoindeskApiService = new Mock<ICoindeskApiService>();
        serviceCollection.AddScoped<ICoindeskApiService>(p => _mockCoindeskApiService.Object);
        
        serviceCollection.AddHttpClient("coindesk",
            c => { c.BaseAddress = new Uri(_configuration.GetSection("Coindesk:Url").Value); });
    }

    private async Task AddInitialData()
    {
        var stubCurrencyRepo = _serviceProvider.GetRequiredService<ICurrencyRepository>() as StubCurrencyRepository;
        await stubCurrencyRepo.RemoveAllAsync();
        var usd = new CurrencyEntity
        {
            Code = "USD",
            CurrencyName = "美金",
            CreateTime = DateTime.Now
        };
        stubCurrencyRepo.Add(usd);

        var gbp = new CurrencyEntity
        {
            Code = "GBP",
            CurrencyName = "英鎊",
            CreateTime = DateTime.Now
        };
        stubCurrencyRepo.Add(gbp);

        var eur = new CurrencyEntity
        {
            Code = "EUR",
            CurrencyName = "歐元",
            CreateTime = DateTime.Now
        };
        stubCurrencyRepo.Add(eur);
        await stubCurrencyRepo.SaveEntitiesAsync();
    }
}