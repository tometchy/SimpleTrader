namespace SimpleTrader.Events;

public class NullMarketUpdated : MarketUpdated
{
    public static MarketUpdated Instance { get; } = new NullMarketUpdated();
    private NullMarketUpdated() : base(string.Empty, DateTime.MinValue, 0m)
    {
    }
}