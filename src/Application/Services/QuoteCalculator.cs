using Crypto.Application.IContracts;
using Crypto.Domain.Entities;

namespace Crypto.Application.Services;
public class QuoteCalculator
{
    private readonly ICoinMarketCapServiceAgent _coinService;
    private readonly IExchangeRateServiceAgent _rateService;

    public QuoteCalculator(ICoinMarketCapServiceAgent coinService, IExchangeRateServiceAgent rateService)
    {
        _coinService = coinService;
        _rateService = rateService;
    }

    public async Task<QuoteResult?> GetQuoteAsync(string cryptoCode)
    {
        var eurPrice = await _coinService.GetEurPriceAsync(cryptoCode);
        if (eurPrice == null) return null;

        var rates = await _rateService.GetRatesAsync();
        return new QuoteResult
        {
            CryptoCurrencyName = cryptoCode.ToUpper(),
            Quotes = rates.ToDictionary(r => r.Key, r => eurPrice.Value * r.Value)
        };
    }
}

