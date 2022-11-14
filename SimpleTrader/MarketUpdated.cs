namespace SimpleTrader;

public class MarketUpdated
{
    public string PairTicker { get; }
    public DateTime Timestamp { get; }
    public decimal LastTradePrice { get; }

    public MarketUpdated(string pairTicker, DateTime timestamp, decimal lastTradePrice)
    {
        PairTicker = pairTicker;
        Timestamp = timestamp;
        LastTradePrice = lastTradePrice;
    }
}