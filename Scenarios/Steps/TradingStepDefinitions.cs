namespace Scenarios.Steps;

[Binding]
public class TradingStepDefinitions
{
    private const string Letters = "[a-zA-Z]*";
    private const string Digits = @"?<=^| )\d+(\.\d+)?(?=$|";

    [Given(@$"the ({Letters}) price is ({Digits}) USD, I bet LONG with ({Digits})% threshold and bought for ({Digits}) USDC")]
    public void LongBetSetup(string ticker, decimal price, decimal threshold, decimal amount)
    {
        ScenarioContext.StepIsPending();
    }

    [Given(@$"the ({Letters}) price is ({Digits}) USD, I bet SHORT with ({Digits})% threshold and sold ({Digits})")]
    public void ShortBetSetup(string ticker, decimal price, decimal threshold, decimal amount)
    {
        ScenarioContext.StepIsPending();
    }

    [When(@$"the price goes to ({Digits}) USD")]
    public void PriceChanging(int p0)
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@$"({Digits}) ({Letters}) is sold for ({Digits}) USDC")]
    public void LongBetClosing(decimal cryptoAmountToSell, string ticker, decimal dollarsAmountToGet)
    {
        ScenarioContext.StepIsPending();
    }

    [Then(@$"({Digits}) USDC is sold for ({Digits}) ({Letters})")]
    public void ShortBetClosing(decimal dollarsAmountToSell, decimal cryptoAmountToGet, string ticker)
    {
        ScenarioContext.StepIsPending();
    }
}