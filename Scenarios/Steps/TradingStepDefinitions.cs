using System.Globalization;
using Akka.Actor;
using Akka.TestKit;
using Moq;
using SimpleTrader;
using TestsUtilities;
using static System.TimeSpan;
using static SimpleTrader.BetParameters;
using static SimpleTrader.BetParameters.BetType;

namespace Scenarios.Steps;

[Binding]
public class TradingStepDefinitions : TestKitWithLog
{
    private IActorRef _app;
    private readonly Queue<decimal> _fakePricesProvider = new();
    private const string Letters = "[a-zA-Z]*";
    private const string Digits = @"\d*\.?\d*";
    private readonly TimeSpan _checkingPriceInterval = FromSeconds(10);
    private Mock<IKrakenClientAdapter> _krakenMock;

    [Given($"({Letters}) price is ({Digits}) USD, LONG bet, ({Digits})% threshold and bought for ({Digits}) USDC")]
    public void SetupLongBet(string ticker, decimal price, decimal threshold, decimal amount) =>
        SetupBet(ticker, price, threshold, amount, Long);

    [Given(@$"({Letters}) price is ({Digits}) USD, SHORT bet, ({Digits})% threshold and sold ({Digits})")]
    public void SetupShortBet(string ticker, decimal price, decimal threshold, decimal amount) =>
        SetupBet(ticker, price, threshold, amount, Short);

    private void SetupBet(string ticker, decimal price, decimal threshold, decimal amount, BetType type)
    {
        var bet = new BetParameters(F.Create<string>(), ticker, type.ToString(),
            price.ToString(CultureInfo.InvariantCulture),
            threshold.ToString(CultureInfo.InvariantCulture), $"{_checkingPriceInterval.TotalSeconds}s",
            amount.ToString(CultureInfo.InvariantCulture));

        _krakenMock = new Mock<IKrakenClientAdapter>();
        _fakePricesProvider.Enqueue(price);
        _app = Sys.ActorOf(Props.Create(() => new App(_krakenMock.Object, bet)));
        _krakenMock.Setup(client => client.GetAssetPrice(It.IsAny<string>())).Returns(_fakePricesProvider.Dequeue);
    }

    [When(@$"the price goes to ({Digits}) USD")]
    public void UpdatePrice(decimal newPrice)
    {
        _fakePricesProvider.Enqueue(newPrice);
        ((TestScheduler)Sys.Scheduler).Advance(FromMilliseconds(_checkingPriceInterval.TotalMilliseconds * 1.5));
    }

    [Then(@$"({Digits}) ({Letters}) is sold for ({Digits}) USDC")]
    public void CloseLongBet(decimal cryptoAmountToSell, string ticker, decimal dollarsAmountToGet)
    {
        _krakenMock.AsyncVerify(f => f.PublishLimitSellOrder(ticker, cryptoAmountToSell,
                dollarsAmountToGet / cryptoAmountToSell), Times.Exactly(1), FromSeconds(1))
            .Wait();
    }

    [Then(@$"({Digits}) USDC is sold for ({Digits}) ({Letters})")]
    public void CloseShortBet(decimal dollarsAmountToSell, decimal cryptoAmountToGet, string ticker)
    {
        ScenarioContext.StepIsPending();
    }
}