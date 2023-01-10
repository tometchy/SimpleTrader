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
        public App(IExchangeClient krakenClient, KrakenSocketClient krakenSocketClient)
        {
            CreateMarketWatcher(Context.ActorOf(Props.Create(() => new BetsSupervisor()), nameof(BetsSupervisor)));

            void CreateMarketWatcher(IActorRef marketUpdatesListener)
            {
                var childProps = Props.Create(() => new RealtimeKrakenWatcher(krakenClient, krakenSocketClient, marketUpdatesListener));
                var supervisor = BackoffSupervisor.Props(Backoff.OnFailure(childProps,
                    childName: nameof(RealtimeKrakenWatcher),
                    minBackoff: TimeSpan.FromSeconds(3),
                    maxBackoff: TimeSpan.FromSeconds(30),
                    randomFactor: 0.2,
                    maxNrOfRetries: -1));
                Context.ActorOf(supervisor, $"{nameof(RealtimeKrakenWatcher)}Supervisor");
            }
        }

        protected override SupervisorStrategy SupervisorStrategy() => new AllForOneStrategy(e => Directive.Stop);
    }
}