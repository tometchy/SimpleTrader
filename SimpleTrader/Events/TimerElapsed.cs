using Akka.Actor;

namespace SimpleTrader.Events;

public class TimerElapsed : ReceiveActor
{
    public static TimerElapsed Instance { get; } = new();
    private TimerElapsed(){}
}