using static Akka.Configuration.ConfigurationFactory;

namespace SimpleTrader;

public class BetParameters
{
    public string Id { get; }
    public string CryptoTicker { get; }
    public BetType Type { get; }
    public decimal InitialPriceUsd { get; }
    public decimal Threshold { get; }
    public TimeSpan PriceCheckInterval { get; }
    public decimal CryptoAmount { get; }

    public BetParameters(string id, string cryptoTicker, string betType, string initialPriceUsd, string threshold,
        string priceCheckInterval, string cryptoAmount)
    {
        Id = id;
        CryptoTicker = cryptoTicker;
        Type = Enum.Parse<BetType>(betType);
        InitialPriceUsd = decimal.Parse(initialPriceUsd);
        Threshold = decimal.Parse(threshold);
        PriceCheckInterval = ParseString($"i = {priceCheckInterval}").GetTimeSpan("i");
        CryptoAmount = decimal.Parse(cryptoAmount);
    }

    public enum BetType
    {
        Long,
        Short
    }
}