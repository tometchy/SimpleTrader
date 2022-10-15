namespace Scenarios.Steps;

[Binding]
public class TradingStepDefinitions
{
    [Given(@"the (.*) price is (.*) USD")]
    public void GivenTheBtcPriceIsUsd(string cryptoTicker, decimal priceInUsd)
    {
        ScenarioContext.StepIsPending();
    }
}