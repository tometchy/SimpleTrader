using static System.StringComparison;

namespace SimpleTrader.Exchange;

public class TradablePairsTickers
{
    public string[] PairsTickers { get; }
    
    public TradablePairsTickers(IEnumerable<string> pairsTickers) =>
        PairsTickers = pairsTickers
            .Where(n => n.EndsWith("/USD") && !n.Contains("usdt", InvariantCultureIgnoreCase))
            .ToArray();
}