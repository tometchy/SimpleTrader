using System.Globalization;
using Akka.Actor;
using Akka.TestKit.NUnit3;
using Moq;
using SimpleTrader;
using TestsUtilities;
using static SimpleTrader.BetParameters;
using static SimpleTrader.BetParameters.BetType;

namespace Scenarios.Steps;

[Binding]
public class TradingStepDefinitions : TestKit
{
    private IActorRef _app;
    private const string Letters = "[a-zA-Z]*";
    private const string Digits = @"\d*\.?\d*";

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
            threshold.ToString(CultureInfo.InvariantCulture), "1s",
            amount.ToString(CultureInfo.InvariantCulture));
        
        _app = Sys.ActorOf(Props.Create(() => new App(Mock.Of<IKrakenClientAdapter>(), bet)));
    }

    [When(@$"the price goes to ({Digits}) USD")]
    public void UpdatePrice(decimal newPrice)
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@$"({Digits}) ({Letters}) is sold for ({Digits}) USDC")]
    public void CloseLongBet(decimal cryptoAmountToSell, string ticker, decimal dollarsAmountToGet)
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@$"({Digits}) USDC is sold for ({Digits}) ({Letters})")]
    public void CloseShortBet(decimal dollarsAmountToSell, decimal cryptoAmountToGet, string ticker)
    {
        ScenarioContext.StepIsPending();
    }
}