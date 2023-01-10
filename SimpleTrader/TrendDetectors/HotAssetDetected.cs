namespace SimpleTrader.TrendDetectors;

public class HotAssetDetected
{
    public DateTime Timestamp { get; }
    public decimal LastPrice { get; }
    public string DetectorId { get; }
    public string PairTicker { get; }
    public string Summary => $"{Timestamp:yyyy-M-dd_HH-mm-ss.fff}_{PairTicker.Replace("/", "_")}_{LastPrice}_{DetectorId}";

    public HotAssetDetected(DateTime timestamp, decimal lastPrice, string detectorId, string pairTicker)
    {
        Timestamp = timestamp;
        LastPrice = lastPrice;
        DetectorId = detectorId;
        PairTicker = pairTicker;
    }

    public override string ToString() => $"[{nameof(HotAssetDetected)} >> {Summary}]";
}