using System.Net.Http.Json;
using Crypto.Application.IContracts;
using Crypto.Infrastructure.Model;
using Crypto.Infrastructure.Models;
using Microsoft.Extensions.Options;


namespace Crypto.Infrastructure.Services;
public class ExchangeRateServiceAgent : IExchangeRateServiceAgent
{

    private readonly HttpClient _httpClient;
    private readonly ExternalServicesSetting _settings;
    private static readonly string[] TargetCurrencies = { "EUR", "GBP", "USD", "BRL", "AUD" };

    public ExchangeRateServiceAgent(HttpClient httpClient, IOptions<ExternalServicesSetting> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<Dictionary<string, decimal>> GetRatesAsync()
    {
        string url = $"{_settings.ExchangeRateApi.BaseUrl}/latest?access_key={_settings.ExchangeRateApi.Key}&symbols={string.Join(",", TargetCurrencies)}";

        var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(url);

        if (response is null)
            throw new HttpRequestException("Empty response from exchange rate API.");

        if (!response.Success || response.Rates is null)
            throw new InvalidOperationException("Failed to fetch rates from exchange rate API.");


        return TargetCurrencies.ToDictionary(
            c => c,
            c =>
            {
                if (!response.Rates.TryGetValue(c, out var rate))
                    throw new KeyNotFoundException($"Rate for currency '{c}' not found.");
                return rate;
            });

    }

}
