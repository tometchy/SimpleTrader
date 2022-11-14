namespace SimpleTrader.Events;

public class TrendDetected
{
    public BetType BetType { get; }
    public decimal LastPrice { get; }

    public TrendDetected(BetType betType, decimal lastPrice)
    {
        BetType = betType;
        LastPrice = lastPrice;
    }
}