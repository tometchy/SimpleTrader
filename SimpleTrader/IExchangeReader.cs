namespace SimpleTrader;

public interface IExchangeReader
{
    Task<decimal> GetAssetPrice(string ticker);
}