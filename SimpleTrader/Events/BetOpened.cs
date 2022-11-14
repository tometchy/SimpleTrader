namespace SimpleTrader.Events;

public class BetOpened
{
    public static BetOpened Instance { get; } = new BetOpened();

    private BetOpened()
    {
    }
}