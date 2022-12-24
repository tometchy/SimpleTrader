namespace SimpleTrader.Exchange;

public class LastTradeData
{
    public decimal Price { get; }
    public decimal Quantity { get; }

    public LastTradeData(decimal price, decimal quantity)
    {
        Price = price;
        Quantity = quantity;
    }
}