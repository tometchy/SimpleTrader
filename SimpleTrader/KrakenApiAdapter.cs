using Kraken.Net.Clients;

namespace SimpleTrader;

public class KrakenClientAdapter : IKrakenClientAdapter
{
    private readonly KrakenClient _client;

    public KrakenClientAdapter(KrakenClient client) => _client = client;
    
    public decimal GetAssetPrice(string ticker)
    {
        var tickerData = _client.SpotApi.ExchangeData.GetTickerAsync(ticker).Result;
        return tickerData.Data.First().Value.LastTrade.Price;
    }
}