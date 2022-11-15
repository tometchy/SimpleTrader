using Akka.Actor;
using SimpleTrader.Events;

namespace SimpleTrader;

public class BetsSupervisor : ReceiveActor
{
    public BetsSupervisor(IExchangeReader exchange)
    {
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(TimeSpan.FromMinutes(5), 1)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_5_1");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(TimeSpan.FromMinutes(5), 1.5m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_5_1.5");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(TimeSpan.FromMinutes(5), 2)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_5_2");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(TimeSpan.FromMinutes(10), 1)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_10_1");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(TimeSpan.FromMinutes(10), 1.5m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_10_1.5");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(TimeSpan.FromMinutes(10), 2)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_10_2");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(TimeSpan.FromMinutes(15), 1)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_15_1");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(TimeSpan.FromMinutes(15), 1.5m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_15_1.5");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(TimeSpan.FromMinutes(15), 2)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_15_2");

        Receive<MarketUpdated>(m => Context.ActorSelection("*").Tell(m, Sender));

        Receive<TrendDetected>(t => Context.ActorOf(Props.Create(() => new Bet(t, exchange)), nameof(Bet) + $"_{t.Id}"));
    }
}