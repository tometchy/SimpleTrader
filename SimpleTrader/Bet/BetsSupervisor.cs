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
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 0.75m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_5_0.75");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 1m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_5_1");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 1.25m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_5_1.25");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 1.5m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_5_1.5");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 1.75m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_5_1.75");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(5), 2m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_5_2");
        //
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 0.75m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_3_0.75");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 1m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_3_1");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 1.25m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_3_1.25");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 1.5m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_3_1.5");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 1.75m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_3_1.75");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(60), FromMinutes(3), 2m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_60_3_2");
        //
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 0.75m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_90_3_0.75");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 1m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_90_3_1");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 1.25m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_90_3_1.25");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 1.5m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_90_3_1.5");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 1.75m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_90_3_1.75");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(90), FromMinutes(3), 2m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_90_3_2");
        //
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 0.75m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_30_3_0.75");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 1m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_30_3_1");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 1.25m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_30_3_1.25");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 1.5m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_30_3_1.5");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 1.75m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_30_3_1.75");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(30), FromMinutes(3), 2m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_30_3_2");
        //
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 0.75m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_15_3_0.75");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 1m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_15_3_1");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 1.25m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_15_3_1.25");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 1.5m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_15_3_1.5");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 1.75m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_15_3_1.75");
        // Context.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(15), FromMinutes(3), 2m)),
        //     nameof(RectangleMinMaxTrendDetector) + "_15_3_2");
        //
        // CreateFixedPercentageFixedLookBackTrendDetector(FromMinutes(0.5), 4.75m);
        // CreateFixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 4.75m);
        // CreateFixedPercentageFixedLookBackTrendDetector(FromMinutes(2), 4.75m);
        // CreateFixedPercentageFixedLookBackTrendDetector(FromMinutes(2), 7.77m);

        // CreateExtraFixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 3.75m, FromMinutes(20));
        CreateExtraFixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 2.75m, FromMinutes(60));
        // CreateExtraFixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 2.75m, FromMinutes(90));
        // CreateExtraFixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 2.75m, FromHours(2));
        // CreateExtraFixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 2.75m, FromHours(4));

        // CreateExtraFixedPercentageFixedLookBackTrendDetector(FromMinutes(4), 4.75m, FromHours(4));
        // CreateExtraFixedPercentageFixedLookBackTrendDetector(FromMinutes(4), 5.75m, FromHours(4));
        CreateExtraFixedPercentageFixedLookBackTrendDetector(FromMinutes(7), 7.77m, FromHours(7));

        // void CreateFixedPercentageFixedLookBackTrendDetector(TimeSpan howLongToLookBack, decimal percentageToCross) =>
        //     Context.ActorOf(Props.Create(() => new FixedPercentageFixedLookBackTrendDetector(howLongToLookBack, percentageToCross)),
        //         nameof(FixedPercentageFixedLookBackTrendDetector) + $"_{howLongToLookBack}_{percentageToCross}");

        void CreateExtraFixedPercentageFixedLookBackTrendDetector(TimeSpan howLongToLookBack, decimal percentageToCross,
            TimeSpan howLongPriceNotSeenBefore) =>
            Context.ActorOf(
                Props.Create(() =>
                    new ExtraFixedPercentageFixedLookBackTrendDetector(howLongToLookBack, percentageToCross, howLongPriceNotSeenBefore)),
                nameof(ExtraFixedPercentageFixedLookBackTrendDetector) +
                $"_{howLongToLookBack}_{percentageToCross}_{howLongPriceNotSeenBefore}");

        Receive<MarketUpdated>(m =>
        {
            Context.GetLogger().Debug($"Propagating market update: {m}");
            Context.ActorSelection("*").Tell(m, Sender);
        });

        Receive<TrendDetected>(t => Context.ActorOf(Props.Create(() => new Bet(t, exchange)), nameof(Bet) + $"_{t.Id}"));
    }
}