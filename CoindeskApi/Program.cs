using CoindeskApi.Filters;
using CoindeskApi.Middlewares;
using CoindeskApi.Models.Domains;
using CoindeskApi.Models.Validators;
using CoindeskApi.Repositories;
using CoindeskApi.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace CoindeskApi;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var isUseInMemoryDatabase = builder.Configuration.GetValue<bool>("ConnectionStrings:IsUseInMemoryDatabase");

        builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add<ExceptionFilter>();
                opt.Filters.Add<ValidateModelFilter>();
            })
            .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);
        builder.Services.AddFluentValidationAutoValidation(opt => opt.DisableDataAnnotationsValidation = true);
        
        if (isUseInMemoryDatabase)
        {
            builder.Services.AddDbContext<CurrencyContext>(opt =>
                opt.UseInMemoryDatabase("Currency"), contextLifetime: ServiceLifetime.Singleton);
        }
        else
        {
            builder.Services.AddDbContext<CurrencyContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        }
        // Add services to the container.
        // // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"CoindeskApi.xml"));
        });

        builder.Services.AddValidatorsFromAssemblyContaining<CreateCurrencyDtoValidator>();
        builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        builder.Services.AddScoped<ICurrencyService, CurrencyService>();
        builder.Services.AddScoped<ICoindeskApiService, CoindeskApiService>();

        builder.Services.AddHttpClient("coindesk",
            httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.Configuration.GetSection("Coindesk:Url").Value);
            });

        var app = builder.Build();
        
        app.UseMiddleware<ApiLogMiddleware>();

        if (isUseInMemoryDatabase)
        {
            InitialData(app);
        }

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
            CurrencyName = "美金",
            CreateTime = DateTime.Now
        };
        currencyContext.Currencies.Add(currency1);

        var currency2 = new CurrencyEntity
        {
            Code = "GBP",
            CurrencyName = "英鎊",
            CreateTime = DateTime.Now
        };
        currencyContext.Currencies.Add(currency2);

        var currency3 = new CurrencyEntity
        {
            Code = "EUR",
            CurrencyName = "歐元",
            CreateTime = DateTime.Now
        };
        currencyContext.Currencies.Add(currency3);
        currencyContext.SaveChanges();
    }
}