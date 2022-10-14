public class BetParameters
{
    public string CryptoTicker { get; }
    public BetType Type { get; }
    public decimal InitialPriceUsd { get; }
    public double Threshold { get; }
    public double Amount { get; }

    public BetParameters(string cryptoTicker, string betType, string initialPriceUsd, string threshold, string amount)
    {
        // CryptoTicker = cryptoTicker;
        // Type = betType;
        // InitialPriceUsd = initialPriceUsd;
        // Threshold = threshold;
        // Amount = amount;
    }

    public enum BetType
    {
        Long,
        Short
    }
}