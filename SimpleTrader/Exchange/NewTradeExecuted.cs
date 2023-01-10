namespace SimpleTrader.Exchange;

public class NewTradeExecuted
{
    public string Exchange { get; }
    public string PairTicker { get; }
    public DateTime Timestamp { get; }
    public decimal LastTradePrice { get; }
    public decimal LastTradeQuantity { get; }

    public NewTradeExecuted(string pairTicker, DateTime timestamp, decimal lastTradePrice, decimal lastTradeQuantity)
    {
        Exchange = "Kraken";
        PairTicker = pairTicker;
        Timestamp = timestamp;
        LastTradePrice = lastTradePrice;
        LastTradeQuantity = lastTradeQuantity;
    }

    public override string ToString() =>
        $"[{nameof(NewTradeExecuted)} ({Exchange}) >> {nameof(PairTicker)}: {PairTicker}, {nameof(Timestamp)}: {Timestamp}, {nameof(LastTradePrice)}: {LastTradePrice}, {nameof(LastTradeQuantity)}: {LastTradeQuantity}]";
}