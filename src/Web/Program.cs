

using System.Text.Json.Serialization;
using Crypto.Application.IContracts;
using Crypto.Application.Services;
using Crypto.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

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
app.MapControllers();
app.Run();
