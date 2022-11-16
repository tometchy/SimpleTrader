using Akka.Actor;
using Akka.Event;
using SimpleTrader.Bet;
using SimpleTrader.Events;

namespace SimpleTrader.TrendDetectors;

public class FixedPercentageFixedLookBackTrendDetector : ReceiveActor
{
    readonly List<MarketUpdated> _updates = new();

    public FixedPercentageFixedLookBackTrendDetector(TimeSpan howLongToLookBack, decimal percentageToCross)
    {
        Context.GetLogger().Info($"Creating trend detector: {Context.Self.Path.Name}");

        Receive<MarketUpdated>(theNewest =>
        {
            _updates.Add(theNewest);

            if (_updates.First().Timestamp > theNewest.Timestamp.Subtract(howLongToLookBack))
                return;

            foreach (var old in _updates.AsEnumerable().Reverse().Skip(1))
            {
                if (old.Timestamp < theNewest.Timestamp.Subtract(howLongToLookBack))
                    return;

                if (theNewest.LastTradePrice > old.LastTradePrice / 100m * (100m + percentageToCross))
                {
                    Context.GetLogger().Info($"LONG Bet detected: {theNewest.LastTradePrice} compared to {old.LastTradePrice}");
                    Context.Parent.Tell(new TrendDetected(theNewest.Timestamp, BetType.Long, theNewest.LastTradePrice,
                        Context.Self.Path.Name, theNewest.PairTicker));
                    return;
                }

                if (theNewest.LastTradePrice < old.LastTradePrice / 100m * (100m - percentageToCross))
                {
                    Context.GetLogger().Info($"SHORT Bet detected: {theNewest.LastTradePrice} compared to {old.LastTradePrice}");
                    Context.Parent.Tell(new TrendDetected(theNewest.Timestamp, BetType.Short, theNewest.LastTradePrice,
                        Context.Self.Path.Name, theNewest.PairTicker));
                    return;
                }
            }
        });
    }
}