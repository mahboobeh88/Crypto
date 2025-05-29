namespace Crypto.Application.IContracts;
public interface IExchangeRateServiceAgent
{
    Task<Dictionary<string, decimal>> GetRatesAsync();
}
