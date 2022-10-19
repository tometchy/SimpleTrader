namespace SimpleTrader;

public interface IKrakenClientAdapter
{
    decimal GetAssetPrice(string ticker);
    public void PublishLimitSellOrder(string ticker, decimal cryptoAmountToSell, decimal expectedPrice);
}