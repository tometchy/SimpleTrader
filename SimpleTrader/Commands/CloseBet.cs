namespace SimpleTrader.Commands;

public class CloseBet
{
    public decimal ClosingPrice { get; }

    public CloseBet(decimal closingPrice)
    {
        ClosingPrice = closingPrice;
    }
}