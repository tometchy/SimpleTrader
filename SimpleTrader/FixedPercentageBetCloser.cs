using Akka.Actor;
using Akka.Event;
using SimpleTrader.Commands;
using SimpleTrader.Events;

namespace SimpleTrader;

public class FixedPercentageBetCloser : ReceiveActor
{
    public FixedPercentageBetCloser(TrendDetected trend, decimal percentage)
    {
        Receive<decimal>(newPrice =>
        {
            Context.GetLogger().Info($"Received new price: {newPrice}");

            if (trend.BetType == BetType.Long && newPrice < trend.LastPrice)
            {
                Context.GetLogger().Info($"{trend.BetType} bet stop loss ({trend.LastPrice}) crossed, closing - SELLING IMMEDIATELY");
                Context.Parent.Tell(new CloseBet(newPrice));
                Self.Tell(PoisonPill.Instance);
            }

            if (trend.BetType == BetType.Short && newPrice > trend.LastPrice)
            {
                Context.GetLogger().Info($"{trend.BetType} bet stop loss ({trend.LastPrice}) crossed, closing - BUYING IMMEDIATELY");
                Context.Parent.Tell(new CloseBet(newPrice));
                Self.Tell(PoisonPill.Instance);
            }
        });
    }
}