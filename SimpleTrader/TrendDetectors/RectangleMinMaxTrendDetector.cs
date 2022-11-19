using Akka.Actor;
using Akka.Event;
using SimpleTrader.Bet;
using SimpleTrader.Events;

namespace SimpleTrader.TrendDetectors;

public class RectangleMinMaxTrendDetector : ReceiveActor
{
    readonly List<MarketUpdated> _updates = new();

    public RectangleMinMaxTrendDetector(TimeSpan howLongToLookBack, TimeSpan howLongToSkipLastPricesFromRectangle,
        decimal howManyTimesRectangleMultiplied)
    {
        Context.GetLogger().Info($"Creating trend detector: {Context.Self.Path.Name}");

        Receive<MarketUpdated>(theNewest =>
        {
            _updates.Add(theNewest);

            if (_updates.First(u => string.Equals(u.PairTicker, theNewest.PairTicker, StringComparison.InvariantCultureIgnoreCase))
                    .Timestamp > theNewest.Timestamp.Subtract(howLongToLookBack))
                return;

            var theLowestPriceInRectangle = 0m;
            var theHighestPriceInRectangle = 0m;
            foreach (var old in _updates.Where(u => string.Equals(u.PairTicker, theNewest.PairTicker, StringComparison.InvariantCultureIgnoreCase))
                         .Reverse()
                         .SkipWhile(o => o.Timestamp > theNewest.Timestamp.Subtract(howLongToSkipLastPricesFromRectangle)))
            {
                if (old.Timestamp < theNewest.Timestamp.Subtract(howLongToLookBack))
                    return;

                if (old.LastTradePrice > theHighestPriceInRectangle)
                    theHighestPriceInRectangle = old.LastTradePrice;
                else if (old.LastTradePrice < theLowestPriceInRectangle)
                    theLowestPriceInRectangle = old.LastTradePrice;
            }

            var rectangleHeight = theHighestPriceInRectangle - theLowestPriceInRectangle;

            if (theNewest.LastTradePrice > theHighestPriceInRectangle + rectangleHeight * howManyTimesRectangleMultiplied)
            {
                Context.GetLogger()
                    .Info($"LONG Bet detected from {theNewest}; Rectangle height: {rectangleHeight}, " +
                          $"the lowest price in rectangle: {theLowestPriceInRectangle}, " +
                          $"the highest price in rectangle: {theHighestPriceInRectangle}");
                Context.Parent.Tell(new TrendDetected(theNewest.Timestamp, BetType.Long, theNewest.LastTradePrice, Context.Self.Path.Name,
                    theNewest.PairTicker));
                return;
            }

            if (theNewest.LastTradePrice < theLowestPriceInRectangle - rectangleHeight * howManyTimesRectangleMultiplied)
            {
                Context.GetLogger()
                    .Info($"SHORT Bet detected from {theNewest}; Rectangle height: {rectangleHeight}, " +
                          $"the lowest price in rectangle: {theLowestPriceInRectangle}, " +
                          $"the highest price in rectangle: {theHighestPriceInRectangle}");
                Context.Parent.Tell(new TrendDetected(theNewest.Timestamp, BetType.Short, theNewest.LastTradePrice, Context.Self.Path.Name,
                    theNewest.PairTicker));
                return;
            }
        });
    }
}