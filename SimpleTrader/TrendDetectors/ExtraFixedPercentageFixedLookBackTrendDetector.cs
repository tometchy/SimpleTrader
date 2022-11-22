using Akka.Actor;
using Akka.Event;
using SimpleTrader.Bet;
using SimpleTrader.Events;
using static System.StringComparison;

namespace SimpleTrader.TrendDetectors;

public class ExtraFixedPercentageFixedLookBackTrendDetector : ReceiveActor
{
    readonly List<MarketUpdated> _updates = new();

    public ExtraFixedPercentageFixedLookBackTrendDetector(TimeSpan howLongToLookBack, decimal percentageToCross,
        TimeSpan howLongPriceNotSeenBefore)
    {
        Context.GetLogger().Info($"Creating trend detector: {Context.Self.Path.Name}");

        Receive<MarketUpdated>(theNewest =>
        {
            _updates.Add(theNewest);

            if (_updates.First(u => string.Equals(u.PairTicker, theNewest.PairTicker, InvariantCultureIgnoreCase))
                    .Timestamp > theNewest.Timestamp.Subtract(howLongPriceNotSeenBefore))
                return;

            foreach (var old in _updates
                         .Where(u => string.Equals(u.PairTicker, theNewest.PairTicker, InvariantCultureIgnoreCase))
                         .Reverse()
                         .Skip(1))
            {
                if (old.Timestamp < theNewest.Timestamp.Subtract(howLongToLookBack))
                    return;

                if (theNewest.LastTradePrice > old.LastTradePrice / 100m * (100m + percentageToCross))
                {
                    if (_updates.Where(u =>
                                string.Equals(u.PairTicker, theNewest.PairTicker, InvariantCultureIgnoreCase) &&
                                u.Timestamp > theNewest.Timestamp.Subtract(howLongPriceNotSeenBefore))
                            .Max(u => u.LastTradePrice) > theNewest.LastTradePrice)
                    {
                        Context.GetLogger()
                            .Debug($"ALMOST LONG Bet detected: {theNewest.LastTradePrice} compared " +
                                   $"to {old.LastTradePrice}, but such price was within {howLongPriceNotSeenBefore}");
                        return;
                    }

                    Context.GetLogger().Info($"LONG Bet detected: {theNewest.LastTradePrice} compared to {old.LastTradePrice}");
                    Context.Parent.Tell(new TrendDetected(theNewest.Timestamp, BetType.Long, theNewest.LastTradePrice,
                        Context.Self.Path.Name, theNewest.PairTicker));
                    return;
                }

                if (theNewest.LastTradePrice < old.LastTradePrice / 100m * (100m - percentageToCross))
                {
                    if (_updates.Where(u =>
                                string.Equals(u.PairTicker, theNewest.PairTicker, InvariantCultureIgnoreCase) &&
                                u.Timestamp > theNewest.Timestamp.Subtract(howLongPriceNotSeenBefore))
                            .Min(u => u.LastTradePrice) < theNewest.LastTradePrice)
                    {
                        Context.GetLogger()
                            .Debug($"ALMOST SHORT Bet detected: {theNewest.LastTradePrice} compared " +
                                   $"to {old.LastTradePrice}, but such price was within {howLongPriceNotSeenBefore}");
                        return;
                    }

                    Context.GetLogger().Info($"SHORT Bet detected: {theNewest.LastTradePrice} compared to {old.LastTradePrice}");
                    Context.Parent.Tell(new TrendDetected(theNewest.Timestamp, BetType.Short, theNewest.LastTradePrice,
                        Context.Self.Path.Name, theNewest.PairTicker));
                    return;
                }
            }
        });
    }
}