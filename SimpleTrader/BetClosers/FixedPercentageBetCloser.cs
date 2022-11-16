using Akka.Actor;
using Akka.Event;
using SimpleTrader.Commands;
using SimpleTrader.Events;
using static SimpleTrader.Bet.BetType;

namespace SimpleTrader.BetClosers;

public class FixedPercentageBetCloser : ReceiveActor
{
    public FixedPercentageBetCloser(TrendDetected trend, decimal percentage)
    {
        var theBiggestObservedPrice = trend.LastPrice;
        var theSmallestObservedPrice = trend.LastPrice;
        
        Receive<MarketUpdated>(update =>
        {
            if (update.LastTradePrice > theBiggestObservedPrice)
                theBiggestObservedPrice = update.LastTradePrice;
            else if (update.LastTradePrice < theSmallestObservedPrice)
                theSmallestObservedPrice = update.LastTradePrice;

            if (trend.BetType == Long && update.LastTradePrice < theBiggestObservedPrice / 100m * (100m - percentage) ||
                trend.BetType == Short && update.LastTradePrice > theSmallestObservedPrice / 100m * (100m + percentage))
            {
                Context.GetLogger().Info($"Commanding bet close, {trend} - {update}");
                Context.Parent.Tell(new CloseBet(update.LastTradePrice));
            }
        });
    }
}