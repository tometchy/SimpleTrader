namespace SimpleTrader.Exchange;

public interface IExchangeClient
{
    Task<AccountDataReceived> GetAccountData();
    Task<OrderPublished> PublishOrder(decimal usdToSpend, decimal inPrice, decimal takeProfitPrice);
    Task<OrderPublished> PublishOrder(decimal usdToSpend, decimal inPrice, decimal takeProfit1Price, decimal takeProfit2Price);
    Task<TradablePairsTickers> GetTradablePairsTickers();
}