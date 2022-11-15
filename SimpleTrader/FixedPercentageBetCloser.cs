using Akka.Actor;
using Akka.Event;
using SimpleTrader.Commands;
using SimpleTrader.Events;

namespace SimpleTrader;

public class FixedPercentageBetCloser : ReceiveActor
{
    public FixedPercentageBetCloser(TrendDetected trend, decimal percentage)
    {
        Receive<MarketUpdated>(update =>
        {
            var newPrice = update.LastTradePrice;
            Context.GetLogger().Info($"Received new price: {newPrice}");
        });
    }
}