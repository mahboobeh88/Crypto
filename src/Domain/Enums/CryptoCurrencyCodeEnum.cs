using System.ComponentModel;

namespace Crypto.Domain.Enums;

public enum CryptoCurrencyCodeEnum:byte
{
    [Description("Bitcoin")]
    BTC=0,
    [Description("Ethereum")]
    ETH,
    [Description("Litecoin")]
    LTC,
    [Description("Solana")]
    SOL
}
