using Kraken.Net.Clients;

namespace SimpleTrader.Exchange;

public class KrakenAdapter : IExchangeAdapter
{
    private readonly KrakenClient _client;

    public KrakenAdapter(KrakenClient client) => _client = client;
    
    public async Task<LastTradeData> GetLastTradeData(string pairTicker)
    {
        // https://support.kraken.com/hc/en-us/articles/203053216-Price-terminology
        // Last Traded Price is purely historical and is not the price that a market order will be executed at.
        // Note: I'm aware that I use last trade price, maybe will change that in the future.
        var tickerData = await _client.SpotApi.ExchangeData.GetTickerAsync(pairTicker);
        var lastTrade = tickerData.Data.First().Value.LastTrade;
        return new LastTradeData(lastTrade.Price, lastTrade.Quantity);
    }

    public Task<decimal> GetAssetAmount(string ticker)
    {
        if(ticker.ToLower() == "sol")
            return Task.FromResult<decimal>(1000);
        
        return Task.FromResult<decimal>(10000);
    }

    public async Task PublishLimitSellOrder(string cryptoTicker, string stableTicker, decimal amountToSell, decimal price)
    {
        // https://support.kraken.com/hc/en-us/sections/200577136-Order-types
        // https://support.kraken.com/hc/en-us/articles/203325783-Market-and-limit-orders
        
        // throw new NotImplementedException();
        // _client.SpotApi.Trading.PlaceOrderAsync(,,OrderType.Limit)
    }

    public async Task PublishLimitBuyOrder(string cryptoTicker, string stableTicker, decimal amountToBuy, decimal price)
    {
        // https://support.kraken.com/hc/en-us/sections/200577136-Order-types
        // https://support.kraken.com/hc/en-us/articles/203325783-Market-and-limit-orders
        
        // throw new NotImplementedException();
        // _client.SpotApi.Trading.PlaceOrderAsync(,,OrderType.Limit)
    }

    public async Task SellImmediately(string cryptoTicker, string stableTicker, decimal amountToSell)
    {
        // throw new NotImplementedException();
        
    }

    public async Task BuyImmediately(string cryptoTicker, string stableTicker, decimal amountToBuy)
    {
        // throw new NotImplementedException();
    }
}