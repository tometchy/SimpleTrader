namespace SimpleTrader.Events;

public class MarketUpdated : IEquatable<MarketUpdated>
{
    public string Exchange { get; }
    public string PairTicker { get; }
    public DateTime Timestamp { get; }
    public decimal LastTradePrice { get; }
    public decimal LastTradeQuantity { get; }

    public MarketUpdated(string pairTicker, DateTime timestamp, decimal lastTradePrice, decimal lastTradeQuantity)
    {
        Exchange = "Kraken";
        PairTicker = pairTicker;
        Timestamp = timestamp;
        LastTradePrice = lastTradePrice;
        LastTradeQuantity = lastTradeQuantity;
    }

    public override string ToString() =>
        $"[{nameof(MarketUpdated)} >> {nameof(PairTicker)}: {PairTicker}, {nameof(Timestamp)}: {Timestamp}, {nameof(LastTradePrice)}: {LastTradePrice}, {nameof(LastTradeQuantity)}: {LastTradeQuantity}]";

    public bool Equals(MarketUpdated? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(PairTicker, other.PairTicker, StringComparison.InvariantCultureIgnoreCase) &&
               Timestamp.Equals(other.Timestamp) &&
               LastTradePrice.Equals(other.LastTradePrice);
    }

    public override bool Equals(object? obj)
    {
        if (obj is MarketUpdated e)
            return Equals(e);
        return false;
    }

    public override int GetHashCode() => HashCode.Combine(PairTicker, Timestamp, LastTradePrice);
}