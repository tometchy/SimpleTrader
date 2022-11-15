namespace SimpleTrader.Exchange;

public interface IExchangeReader
{
    Task<decimal> GetAssetPrice(string pairTicker);
    Task<decimal> GetAssetAmount(string ticker);
}