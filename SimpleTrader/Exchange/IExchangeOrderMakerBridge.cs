namespace SimpleTrader.Exchange;

public interface IExchangeOrderMakerBridge
{
    public Task SellImmediately(string cryptoTicker, string stableTicker);
    public Task BuyImmediately(string cryptoTicker, string stableTicker);
}