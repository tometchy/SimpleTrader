namespace SimpleTrader.Exchange;

public class OrderPublished
{
    public string Id { get; }

    public OrderPublished(string id) => Id = id;
}