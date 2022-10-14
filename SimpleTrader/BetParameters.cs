using static SimpleTrader.BetParameters.BetType;

namespace SimpleTrader;

public class BetParameters
{
    public string CryptoTicker { get; }
    public BetType Type { get; }
    public decimal InitialPriceUsd { get; }
    public double Threshold { get; }
    public double Amount { get; }

    public BetParameters(string cryptoTicker, string betType, string initialPriceUsd, string threshold, string amount)
    {
        CryptoTicker = cryptoTicker;
        Type = Enum.Parse<BetType>(betType);
        InitialPriceUsd = decimal.Parse(initialPriceUsd);
        Threshold = double.Parse(threshold);
        Amount = double.Parse(amount);

        if (Type is Long && Amount < 5)
            throw new ArgumentException($"Detected wrong type of bet. " +
                                        $"You declared that {cryptoTicker} price will raise and that you have spent" +
                                        $"{Amount} dollars for it. It's too small to make sense.");

        if (Type is Short && Amount > 5)
            throw new ArgumentException($"Detected wrong type of bet. " +
                                        $"You declared that {cryptoTicker} price will fall and that you have spent" +
                                        $"{Amount} of it. It seems too much for this kind of simple trading.");
    }

    public enum BetType
    {
        Long,
        Short
    }
}