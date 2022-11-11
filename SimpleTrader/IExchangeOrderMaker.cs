namespace SimpleTrader;

public interface IExchangeOrderMaker
{
    public Task PublishLimitSellOrder(string cryptoTicker, string stableTicker, decimal amountToSell, decimal price);
    public Task PublishLimitBuyOrder(string cryptoTicker, string stableTicker, decimal amountToBuy, decimal price);
}