namespace SimpleTrader.Exchange;

public interface IExchangeOrderMaker
{
    // public Task PublishLimitSellOrder(string cryptoTicker, string stableTicker, decimal amountToSell, decimal price);
    // public Task PublishLimitBuyOrder(string cryptoTicker, string stableTicker, decimal amountToBuy, decimal price);
    
    public Task SellImmediately(string cryptoTicker, string stableTicker, decimal amountToSell);
    public Task BuyImmediately(string cryptoTicker, string stableTicker, decimal amountToBuy);
}