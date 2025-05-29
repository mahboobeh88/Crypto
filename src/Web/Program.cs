using System.Text.Json.Serialization;
using Crypto.Application.IContracts;
using Crypto.Application.Services;
using Crypto.Infrastructure.Models;
using Crypto.Infrastructure.Services;
using Crypto.Web.Middlewares;
using Serilog;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Starting up");
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.Configure<ExternalServicesSetting>(builder.Configuration.GetSection("ExternalSettings"));
    builder.Services.AddOptions<ExternalServicesSetting>().ValidateDataAnnotations().ValidateOnStart();
    // Add services
    builder.Services.AddHttpClient();
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<QuoteCalculator>();
    builder.Services.AddScoped<ICoinMarketCapServiceAgent, CoinMarketCapServiceAgent>();
    builder.Services.AddScoped<IExchangeRateServiceAgent, ExchangeRateServiceAgent>();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.MapControllers();
    app.Run();

}
catch (Exception ex)
{
    Log.Fatal($"Start Failed {ex.Message}", ex);
}
