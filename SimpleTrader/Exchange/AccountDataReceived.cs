namespace SimpleTrader.Exchange;

public class AccountDataReceived
{
    public decimal AvailableUsd { get; }

    public AccountDataReceived(decimal availableUsd) => AvailableUsd = availableUsd;
}