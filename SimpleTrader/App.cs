using Akka.Actor;
using Akka.Event;

namespace SimpleTrader
{
    public class App : ReceiveActor
    {
        public App(IExchangeAdapter exchange)
        {
            Context.GetLogger().Info("Creating app");
            // Context.ActorOf(Props.Create(() => new UsersRepository()), nameof(UsersRepository));
        }
    }
}