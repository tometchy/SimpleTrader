using Kraken.Net.Clients;
using Kraken.Net.Enums;

namespace SimpleTrader;

public class KrakenClientAdapter : IKrakenClientAdapter
{
    private readonly KrakenClient _client;

    public KrakenClientAdapter(KrakenClient client) => _client = client;
    
    public decimal GetAssetPrice(string ticker)
    {
        // https://support.kraken.com/hc/en-us/articles/203053216-Price-terminology
        // Last Traded Price is purely historical and is not the price that a market order will be executed at.
        // Note: I'm aware that I use last trade price, maybe will change that in the future.
        var tickerData = _client.SpotApi.ExchangeData.GetTickerAsync(ticker).Result;
        return tickerData.Data.First().Value.LastTrade.Price;
    }

    public void PublishLimitSellOrder(string ticker, decimal cryptoAmountToSell, decimal expectedPrice)
    {
        // https://support.kraken.com/hc/en-us/sections/200577136-Order-types
        // https://support.kraken.com/hc/en-us/articles/203325783-Market-and-limit-orders
        
        throw new NotImplementedException();
        // _client.SpotApi.Trading.PlaceOrderAsync(,,OrderType.Limit)
    }
}