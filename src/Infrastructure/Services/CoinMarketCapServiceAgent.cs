using System.Text.Json;
using Crypto.Application.IContracts;
using Crypto.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Crypto.Infrastructure.Common;

namespace Crypto.Infrastructure.Services;
public class CoinMarketCapServiceAgent : ICoinMarketCapServiceAgent
{
    private readonly HttpClient _httpClient;

    private readonly ExternalServicesSetting _settings;
    
   
    public CoinMarketCapServiceAgent(HttpClient httpClient, IOptions<ExternalServicesSetting> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
       
    }

    public async Task<decimal?> GetEurPriceAsync(string symbol)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", _settings.CoinMarketCapApi.Key);

        var response = await _httpClient.GetAsync($"{_settings.CoinMarketCapApi.BaseUrl}/cryptocurrency/quotes/latest?symbol={symbol}&convert=EUR");

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Failed to fetch eurPrice: {response.StatusCode} - {response}");

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
      

        return ExtractPrice(json, symbol);

    }

    private decimal ExtractPrice(JsonDocument json, string symbol)
    {
        var path = $"data.{symbol.ToUpper()}.quote.EUR.price";

        var priceElement = json.RootElement.TryGetNestedProperty(path.Split('.'));
        
        if (priceElement.ValueKind == JsonValueKind.Null)
            throw new ArgumentNullException("Price Value is Null");

        return priceElement.GetDecimal();
    }
}

