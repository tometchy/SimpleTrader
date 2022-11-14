namespace SimpleTrader.Events;

public class TimerElapsed
{
    public static TimerElapsed Instance { get; } = new TimerElapsed();

    private TimerElapsed()
    {
    }
}