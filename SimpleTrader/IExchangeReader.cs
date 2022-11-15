namespace SimpleTrader;

public interface IExchangeReader
{
    Task<decimal> GetAssetPrice(string pairTicker);
    Task<decimal> GetAssetAmount(string ticker);
}