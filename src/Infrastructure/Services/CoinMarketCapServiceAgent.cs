using Crypto.Application.IContracts;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Crypto.Infrastructure.Services;
public class CoinMarketCapServiceAgent:ICoinMarketCapServiceAgent
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly string _baseUrl;

    public CoinMarketCapServiceAgent(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
        _baseUrl = _config["CoinMarketCapBaseUrl"]??throw new Exception("CoinMarketCap url not found");
    }

    public async Task<decimal?> GetEurPriceAsync(string symbol)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", _config["CoinMarketCapApiKey"]);

            var response = await _httpClient.GetAsync($"{_baseUrl}/cryptocurrency/quotes/latest?symbol={symbol}&convert=EUR");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Failed to fetch eurPrice: {response.StatusCode} - {response}");

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);
            return json.RootElement
                .GetProperty("data")
                .GetProperty(symbol.ToUpper())
                .GetProperty("quote")
                .GetProperty("EUR")
                .GetProperty("price")
                .GetDecimal();

        }
        catch (Exception ex)
        {
            var s = ex;
            throw;
        }
       
    }
}

