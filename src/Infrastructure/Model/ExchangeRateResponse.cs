using System.Text.Json.Serialization;

namespace Crypto.Infrastructure.Model;
internal class ExchangeRateResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("Rates")]
    public Dictionary<string, decimal>? Rates { get; set; }

}
