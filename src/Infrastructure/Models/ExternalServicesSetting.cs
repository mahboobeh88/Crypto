using System.ComponentModel.DataAnnotations;

namespace Crypto.Infrastructure.Models;
public class ExternalServicesSetting
{
    [Required]
    public required BaseExternalApiSettings CoinMarketCapApi { get; set; }
    [Required]
    public required BaseExternalApiSettings ExchangeRateApi { get; set; }

}
