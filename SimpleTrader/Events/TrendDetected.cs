using SimpleTrader.Bet;

namespace SimpleTrader.Events;

public class TrendDetected
{
    public DateTime Timestamp { get; }
    public BetType BetType { get; }
    public decimal LastPrice { get; }
    public string DetectorId { get; }
    public string PairTicker { get; }
    public string Id => $"{Timestamp.ToString("yyyy-M-dd_HH-mm-ss")}_{PairTicker.Replace("/", "_")}_{BetType}_{LastPrice}_{DetectorId}";

    public TrendDetected(DateTime timestamp, BetType betType, decimal lastPrice, string detectorId, string pairTicker)
    {
        Timestamp = timestamp;
        BetType = betType;
        LastPrice = lastPrice;
        DetectorId = detectorId;
        PairTicker = pairTicker;
    }

    public override string ToString() => $"[{nameof(TrendDetected)} >> {Id}]";
}