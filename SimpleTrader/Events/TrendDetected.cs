namespace SimpleTrader.Events;

public class TrendDetected
{
    public DateTime Timestamp { get; }
    public BetType BetType { get; }
    public decimal LastPrice { get; }
    public string Id => $"{Timestamp.ToString("yyyy-dd-M_HH-mm-ss")}_{BetType}_{LastPrice}";

    public TrendDetected(DateTime timestamp, BetType betType, decimal lastPrice)
    {
        Timestamp = timestamp;
        BetType = betType;
        LastPrice = lastPrice;
    }

    public override string ToString() => $"[{nameof(TrendDetected)} >> {nameof(BetType)}: {BetType}, {nameof(LastPrice)}: {LastPrice}]";
}