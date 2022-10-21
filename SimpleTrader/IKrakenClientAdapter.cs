namespace SimpleTrader;

public interface IKrakenClientAdapter
{
    Task<decimal> GetAssetPrice(string ticker);
    public Task PublishLimitSellOrder(string ticker, decimal cryptoAmountToSell, decimal expectedPrice);
    public Task PublishLimitBuyOrder(string ticker, decimal cryptoAmountToBuy, decimal expectedPrice);
}