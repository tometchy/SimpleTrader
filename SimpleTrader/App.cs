using Akka.Actor;

namespace SimpleTrader
{
    public class App : ReceiveActor
    {
        public App()
        {
            // Context.ActorOf(Props.Create(() => new UsersRepository()), nameof(UsersRepository));
        }
    }
}