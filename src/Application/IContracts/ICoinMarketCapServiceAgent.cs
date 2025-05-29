namespace Crypto.Application.IContracts;
public interface ICoinMarketCapServiceAgent
{
    Task<decimal?> GetEurPriceAsync(string symbol);
}
