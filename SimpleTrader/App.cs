using Akka.Actor;
using Akka.Event;
using Akka.Pattern;
using Kraken.Net.Clients;
using SimpleTrader.Bet;
using SimpleTrader.Exchange;

namespace SimpleTrader
{
    public class App : ReceiveActor
    {
        public App(KrakenSocketClient krakenSocketClient, IExchangeReader priceReader, IExchangeOrderMakerBridge orderMaker)
        {
            CreateMarketWatcher(Context.ActorOf(Props.Create(() => new BetsSupervisor(priceReader)), nameof(BetsSupervisor)));

            void CreateMarketWatcher(IActorRef marketUpdatesListener)
            {
                var childProps = Props.Create(() => new RealtimeExchangeWatcher(krakenSocketClient, marketUpdatesListener));
                var supervisor = BackoffSupervisor.Props(Backoff.OnFailure(childProps,
                    childName: nameof(RealtimeExchangeWatcher),
                    minBackoff: TimeSpan.FromSeconds(3),
                    maxBackoff: TimeSpan.FromSeconds(30),
                    randomFactor: 0.2,
                    maxNrOfRetries: -1));
                Context.ActorOf(supervisor, $"{nameof(RealtimeExchangeWatcher)}Supervisor");
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new AllForOneStrategy(e =>
            {
                Context.GetLogger().Error(e, "SOMETHING WENT WRONG, TEMPORARY WORKAROUND");
                return Directive.Resume; // TEMPORARY
            });
        }
    }
}