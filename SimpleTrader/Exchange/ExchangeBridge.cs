namespace SimpleTrader.Exchange;

public class ExchangeBridge : IExchangeOrderMakerBridge
{
    private readonly IExchangeOrderMaker _exchange;

    public ExchangeBridge(IExchangeOrderMaker exchange) => _exchange = exchange;

    public async Task SellImmediately(string cryptoTicker, string stableTicker)
    {
        // throw new NotImplementedException();
    }

    public async Task BuyImmediately(string cryptoTicker, string stableTicker)
    {
        // throw new NotImplementedException();
    }
}