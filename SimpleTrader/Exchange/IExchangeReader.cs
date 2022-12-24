namespace SimpleTrader.Exchange;

public interface IExchangeReader
{
    Task<LastTradeData> GetLastTradeData(string pairTicker);
    Task<decimal> GetAssetAmount(string ticker);
}