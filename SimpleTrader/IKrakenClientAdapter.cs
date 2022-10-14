namespace SimpleTrader;

public interface IKrakenClientAdapter
{
    decimal GetAssetPrice(string ticker);
}