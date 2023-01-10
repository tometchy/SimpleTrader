using Kraken.Net.Clients;

namespace SimpleTrader.Exchange;

public class KrakenClientAdapter : IExchangeClient
{
    private readonly KrakenClient _krakenClient;

    public KrakenClientAdapter(KrakenClient krakenClient) => _krakenClient = krakenClient;

    public Task<AccountDataReceived> GetAccountData()
    {
        throw new NotImplementedException();
    }

    public Task<OrderPublished> PublishOrder(decimal usdToSpend, decimal inPrice, decimal takeProfitPrice)
    {
        throw new NotImplementedException();
    }

    public Task<OrderPublished> PublishOrder(decimal usdToSpend, decimal inPrice, decimal takeProfit1Price, decimal takeProfit2Price)
    {
        throw new NotImplementedException();
    }

    public async Task<TradablePairsTickers> GetTradablePairsTickers()
    {
        var result = await _krakenClient.SpotApi.ExchangeData.GetSymbolsAsync();
        return new TradablePairsTickers(result.Data.Values.Select(symbol => symbol.WebsocketName));
    }
}