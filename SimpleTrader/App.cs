using Akka.Actor;
using Akka.Pattern;
using Kraken.Net.Clients;

namespace SimpleTrader
{
    public class App : ReceiveActor
    {
        public App(KrakenSocketClient krakenSocketClient, IExchangeReader priceReader, IExchangeOrderMakerBridge orderMaker)
        {
            CreateMarketWatcher(Context.ActorOf(Props.Create(() => new BetsSupervisor()), nameof(BetsSupervisor)));

            // Context.System.Scheduler.ScheduleTellRepeatedly(Zero, FromMinutes(1), Self, TimerElapsed.Instance, Self);
            // Receive<TimerElapsed>(_ =>
            // priceReader.GetAssetPrice($"{Crypto}USD").ContinueWith(r => r.Result).PipeTo(Context.ActorSelection("*")));


            // Receive<TrendDetected>(trend => Context.ActorOf(Props.Create(() => new FixedPercentageBetCloser(orderMaker, trend.BetType, trend.LastPrice))));

            void CreateMarketWatcher(IActorRef marketUpdatesListener)
            {
                var childProps = Props.Create(() => new RealtimeMarketWatcher(krakenSocketClient, marketUpdatesListener));
                var supervisor = BackoffSupervisor.Props(Backoff.OnFailure(childProps,
                    childName: nameof(RealtimeMarketWatcher),
                    minBackoff: TimeSpan.FromSeconds(3),
                    maxBackoff: TimeSpan.FromSeconds(30),
                    randomFactor: 0.2));
                Context.ActorOf(supervisor, $"{nameof(RealtimeMarketWatcher)}Supervisor");
            }
        }
    }
}