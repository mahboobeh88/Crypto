using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Azure;
using Crypto.Application.IContracts;
using Crypto.Infrastructure.Model;
using Microsoft.Extensions.Configuration;


namespace Crypto.Infrastructure.Services;
public class ExchangeRateServiceAgent : IExchangeRateServiceAgent
{

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly string _baseUrl;
    private readonly string _accessKey;
    private static readonly string[] TargetCurrencies = { "EUR", "GBP", "USD", "BRL", "AUD" };

    public ExchangeRateServiceAgent(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _config = configuration;
        _baseUrl = _config["ExchangeRateBaseUrl"] ?? throw new ArgumentException("ExchangeRateBaseUrl is not configured");
        _accessKey = _config["ExchangeRateApiKey"] ?? throw new ArgumentException("ExchangeRateKey  is not configured");
    }

    public async Task<Dictionary<string, decimal>> GetRatesAsync()
    {
        try
        {
            
            string url = $"{_baseUrl}/latest?access_key={_accessKey}&symbols={string.Join(",", TargetCurrencies)}";

            var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(url);

            if (response is null)
                throw new HttpRequestException("Empty response from exchange rate API.");

            if (!response.Success || response.Rates is null)
                throw new InvalidOperationException("Failed to retrieve rates from exchange rate API.");


            return TargetCurrencies.ToDictionary(
                c => c,
                c =>
                {
                    if (!response.Rates.TryGetValue(c, out var rate))
                        throw new KeyNotFoundException($"Rate for currency '{c}' not found.");
                    return rate;
                });
        }

        catch (Exception ex)
        {
            var s = ex;
            throw;
        }
    }
    
}
