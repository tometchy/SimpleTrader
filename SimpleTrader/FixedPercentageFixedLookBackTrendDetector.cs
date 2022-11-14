using Akka.Actor;
using Akka.Event;
using SimpleTrader.Events;

namespace SimpleTrader;

public class FixedPercentageFixedLookBackTrendDetector : ReceiveActor
{
    readonly List<MarketUpdated> _updates = new();

    public FixedPercentageFixedLookBackTrendDetector(TimeSpan howLongToLookBack, decimal percentageToCross)
    {
        Receive<MarketUpdated>(theNewestUpdate =>
        {
            Context.GetLogger().Info($"Received new price: {theNewestUpdate}");
            _updates.Add(theNewestUpdate);

            if (_updates.First().Timestamp > theNewestUpdate.Timestamp - howLongToLookBack)
            {
                Context.GetLogger().Info($"Not enough updates for {howLongToLookBack} looking back");
                return;
            }

            foreach (var oldUpdate in _updates.AsEnumerable().Reverse().Skip(1))
            {
                if (oldUpdate.Timestamp < theNewestUpdate.Timestamp - howLongToLookBack)
                    return;

                if (theNewestUpdate.LastTradePrice > (oldUpdate.LastTradePrice / 100) * (100 + percentageToCross))
                {
                    Context.GetLogger().Info($"LONG Bet detected: {theNewestUpdate.LastTradePrice} compared to {oldUpdate.LastTradePrice}");
                    Context.Parent.Tell(new TrendDetected(theNewestUpdate.Timestamp, BetType.Long, theNewestUpdate.LastTradePrice));
                }
                else if (theNewestUpdate.LastTradePrice < (oldUpdate.LastTradePrice / 100) * (100 - percentageToCross))
                {
                    Context.GetLogger().Info($"SHORT Bet detected: {theNewestUpdate.LastTradePrice} compared to {oldUpdate.LastTradePrice}");
                    Context.Parent.Tell(new TrendDetected(theNewestUpdate.Timestamp, BetType.Short, theNewestUpdate.LastTradePrice));
                }
            }
        });
    }
}