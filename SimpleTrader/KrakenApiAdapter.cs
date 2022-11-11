using Kraken.Net.Clients;

namespace SimpleTrader;

public class KrakenAdapter : IExchangeAdapter
{
    private readonly KrakenClient _client;

    public KrakenAdapter(KrakenClient client) => _client = client;
    
    public async Task<decimal> GetAssetPrice(string ticker)
    {
        // https://support.kraken.com/hc/en-us/articles/203053216-Price-terminology
        // Last Traded Price is purely historical and is not the price that a market order will be executed at.
        // Note: I'm aware that I use last trade price, maybe will change that in the future.
        var tickerData = await _client.SpotApi.ExchangeData.GetTickerAsync(ticker);
        return tickerData.Data.First().Value.LastTrade.Price;
    }

    public async Task PublishLimitSellOrder(string cryptoTicker, string stableTicker, decimal amountToSell, decimal price)
    {
        // https://support.kraken.com/hc/en-us/sections/200577136-Order-types
        // https://support.kraken.com/hc/en-us/articles/203325783-Market-and-limit-orders
        
        throw new NotImplementedException();
        // _client.SpotApi.Trading.PlaceOrderAsync(,,OrderType.Limit)
    }

    public async Task PublishLimitBuyOrder(string cryptoTicker, string stableTicker, decimal amountToBuy, decimal price)
    {
        // https://support.kraken.com/hc/en-us/sections/200577136-Order-types
        // https://support.kraken.com/hc/en-us/articles/203325783-Market-and-limit-orders
        
        throw new NotImplementedException();
        // _client.SpotApi.Trading.PlaceOrderAsync(,,OrderType.Limit)
    }
}