using Akka.Actor;
using Akka.Event;
using SimpleTrader.Events;

namespace SimpleTrader.TrendDetectors;

public class FixedPercentageFixedLookBackTrendDetector : ReceiveActor
{
    readonly List<MarketUpdated> _updates = new();

    public FixedPercentageFixedLookBackTrendDetector(TimeSpan howLongToLookBack, decimal percentageToCross)
    {
        var detectorId = $"{nameof(FixedPercentageFixedLookBackTrendDetector)}_{howLongToLookBack}_{percentageToCross}";
        Receive<MarketUpdated>(theNewest =>
        {
            Context.GetLogger().Debug($"Received new price: {theNewest}");
            _updates.Add(theNewest);

            var firstTimestamp = _updates.First().Timestamp;
            var theNewestTimestampSubstracted = theNewest.Timestamp.Subtract(howLongToLookBack);
            if (firstTimestamp > theNewestTimestampSubstracted)
            {
                Context.GetLogger().Debug($"Not enough updates for {howLongToLookBack} looking back; " +
                                          $"First timestamp: {firstTimestamp}; " +
                                          $"The newest timestamp: {theNewest.Timestamp}; " +
                                          $"The newest timestamp susbtracted: {theNewestTimestampSubstracted}");
                return;
            }

            foreach (var old in _updates.AsEnumerable().Reverse().Skip(1))
            {
                if (old.Timestamp < theNewest.Timestamp - howLongToLookBack)
                    return;

                if (theNewest.LastTradePrice > old.LastTradePrice / 100m * (100m + percentageToCross))
                {
                    Context.GetLogger().Info($"LONG Bet detected: {theNewest.LastTradePrice} compared to {old.LastTradePrice}");
                    Context.Parent.Tell(new TrendDetected(theNewest.Timestamp, BetType.Long, theNewest.LastTradePrice, detectorId,
                        theNewest.PairTicker));
                }
                else if (theNewest.LastTradePrice < old.LastTradePrice / 100m * (100m - percentageToCross))
                {
                    Context.GetLogger().Info($"SHORT Bet detected: {theNewest.LastTradePrice} compared to {old.LastTradePrice}");
                    Context.Parent.Tell(new TrendDetected(theNewest.Timestamp, BetType.Short, theNewest.LastTradePrice, detectorId,
                        theNewest.PairTicker));
                }
            }
        });
    }
}