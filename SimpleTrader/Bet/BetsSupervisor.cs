using Akka.Actor;
using Akka.Event;
using SimpleTrader.Events;
using SimpleTrader.Exchange;
using SimpleTrader.TrendDetectors;
using static System.TimeSpan;

namespace SimpleTrader.Bet;

public class BetsSupervisor : ReceiveActor
{
    public BetsSupervisor(IExchangeReader exchange)
    {
        // Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(5), 1)),
        //     nameof(FixedPercentageFixedLookBackTrendDetector) + "_5_1");
        // Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(5), 1.5m)),
        //     nameof(FixedPercentageFixedLookBackTrendDetector) + "_5_1.5");
        // Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(5), 2)),
            // nameof(FixedPercentageFixedLookBackTrendDetector) + "_5_2");
        // Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(10), 1)),
        //     nameof(FixedPercentageFixedLookBackTrendDetector) + "_10_1");
        // Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(10), 1.5m)),
        //     nameof(FixedPercentageFixedLookBackTrendDetector) + "_10_1.5");
        // Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(10), 2)),
            // nameof(FixedPercentageFixedLookBackTrendDetector) + "_10_2");
        // Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(15), 1)),
        //     nameof(FixedPercentageFixedLookBackTrendDetector) + "_15_1");
        // Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(15), 1.5m)),
        //     nameof(FixedPercentageFixedLookBackTrendDetector) + "_15_1.5");
        // Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(15), 2)),
            // nameof(FixedPercentageFixedLookBackTrendDetector) + "_15_2");
        
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 0.75m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_5_0.75");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 1m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_5_1");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 1.25m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_5_1.25");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 1.5m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_5_1.5");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 1.75m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_5_1.75");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 2m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_5_2");
        
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 0.75m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_3_0.75");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 1m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_3_1");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 1.25m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_3_1.25");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 1.5m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_3_1.5");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 1.75m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_3_1.75");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 2m)),
            nameof(RectangleMinMaxTrendDetector) + "_60_3_2");
        
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 0.75m)),
            nameof(RectangleMinMaxTrendDetector) + "_90_3_0.75");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 1m)),
            nameof(RectangleMinMaxTrendDetector) + "_90_3_1");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 1.25m)),
            nameof(RectangleMinMaxTrendDetector) + "_90_3_1.25");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 1.5m)),
            nameof(RectangleMinMaxTrendDetector) + "_90_3_1.5");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 1.75m)),
            nameof(RectangleMinMaxTrendDetector) + "_90_3_1.75");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 2m)),
            nameof(RectangleMinMaxTrendDetector) + "_90_3_2");
        
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 0.75m)),
            nameof(RectangleMinMaxTrendDetector) + "_30_3_0.75");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 1m)),
            nameof(RectangleMinMaxTrendDetector) + "_30_3_1");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 1.25m)),
            nameof(RectangleMinMaxTrendDetector) + "_30_3_1.25");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 1.5m)),
            nameof(RectangleMinMaxTrendDetector) + "_30_3_1.5");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 1.75m)),
            nameof(RectangleMinMaxTrendDetector) + "_30_3_1.75");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 2m)),
            nameof(RectangleMinMaxTrendDetector) + "_30_3_2");
        
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 0.75m)),
            nameof(RectangleMinMaxTrendDetector) + "_15_3_0.75");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 1m)),
            nameof(RectangleMinMaxTrendDetector) + "_15_3_1");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 1.25m)),
            nameof(RectangleMinMaxTrendDetector) + "_15_3_1.25");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 1.5m)),
            nameof(RectangleMinMaxTrendDetector) + "_15_3_1.5");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 1.75m)),
            nameof(RectangleMinMaxTrendDetector) + "_15_3_1.75");
        Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 2m)),
            nameof(RectangleMinMaxTrendDetector) + "_15_3_2");
        
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 2m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_1_2");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 2.25m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_1_2.25");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 2.5m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_1_2.5");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 1.5m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_1_1.5");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 1.75m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_1_1.75");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(0.5), 2m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_0.5_2");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(0.5), 2.25m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_0.5_2.25");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(0.5), 2.5m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_0.5_2.5");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(0.5), 1.5m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_0.5_1.5");
        Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(FromMinutes(0.5), 1.75m)),
            nameof(FixedPercentageFixedLookBackTrendDetector) + "_0.5_1.75");

        Receive<MarketUpdated>(m =>
        {
            Context.GetLogger().Debug($"Propagating market update: {m}");
            Context.ActorSelection("*").Tell(m, Sender);
        });

        Receive<TrendDetected>(t => Context.ActorOf(Props.Create(() => new Bet(t, exchange)), nameof(Bet) + $"_{t.Id}"));
    }
}