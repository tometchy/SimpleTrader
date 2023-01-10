using Akka.Actor;
using Akka.Event;
using SimpleTrader.Exchange;
using SimpleTrader.TrendDetectors;
using static System.TimeSpan;

namespace SimpleTrader.Bet;

public class BetsSupervisor : ReceiveActor
{
    public BetsSupervisor()
    {
        CreateExtraFixedPercentageFixedLookBackTrendDetector(FromMinutes(1), 2.75m, FromMinutes(60));

        CreateExtraFixedPercentageFixedLookBackTrendDetector(FromMinutes(7), 7.77m, FromHours(7));

        void CreateExtraFixedPercentageFixedLookBackTrendDetector(TimeSpan howLongToLookBack, decimal percentageToCross,
            TimeSpan howLongPriceNotSeenBefore) =>
            Context.ActorOf(
                Props.Create(() =>
                    new ExtraFixedPercentageFixedLookBackTrendDetector(howLongToLookBack, percentageToCross, howLongPriceNotSeenBefore)),
                nameof(ExtraFixedPercentageFixedLookBackTrendDetector) +
                $"_{howLongToLookBack}_{percentageToCross}_{howLongPriceNotSeenBefore}");

        Receive<NewTradeExecuted>(m =>
        {
            Context.GetLogger().Debug($"Propagating market update: {m}");
            Context.ActorSelection("*").Tell(m, Sender);
            ChartPublisher.Publish(m);
        });

        Receive<HotAssetDetected>(t =>
        {
            ListPublisher.Publish($"{DateTime.UtcNow} >> [{t}]");
        });
    }
}