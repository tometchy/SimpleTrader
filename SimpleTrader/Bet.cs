using Akka.Actor;
using Akka.Event;
using SimpleTrader.Commands;
using SimpleTrader.Events;

namespace SimpleTrader;

public class Bet : ReceiveActor
{
    private readonly TrendDetected _trend;
    private string BetFilePath => $"/var/simple-trader/{_trend.Id}.txt";

    public Bet(TrendDetected trend)
    {
        _trend = trend;
        Become(OpeningBet);
    }

    private void OpeningBet()
    {
        Context.GetLogger().Info($"Opening bet for {_trend}");
        Self.Tell(BetOpened.Instance); // Exchange will do it
        Receive<BetOpened>(_ => Become(Opened));
    }

    private void Opened()
    {
        File.AppendAllText(BetFilePath, $"Bet opened {_trend}");
        Context.GetLogger().Info($"Bet opened {_trend}");
        var numberOfClosingSimulators = 0;

        Context.ActorOf(Props.Create(() => new FixedPercentageBetCloser(_trend, 1)), nameof(FixedPercentageBetCloser) + "_1");
        numberOfClosingSimulators++;
        Context.ActorOf(Props.Create(() => new FixedPercentageBetCloser(_trend, 0.5m)), nameof(FixedPercentageBetCloser) + "_0.5");
        numberOfClosingSimulators++;
        Context.ActorOf(Props.Create(() => new FixedPercentageBetCloser(_trend, 1.5m)), nameof(FixedPercentageBetCloser) + "_1.5");
        numberOfClosingSimulators++;

        Receive<MarketUpdated>(m => Context.ActorSelection("*").Tell(m, Sender));

        var numberOfClosedBets = 0;
        Receive<CloseBet>(c =>
        {
            var revenueInTheory = _trend.BetType == BetType.Long
                ? $"{c.ClosingPrice * 100 / _trend.LastPrice - 100}%"
                : $"{_trend.LastPrice * 100 / c.ClosingPrice - 100}%";

            File.AppendAllText(BetFilePath, $"Closing {_trend} as {Sender.Path.ToString()} suggests; REVENUE IN THEORY: {revenueInTheory}");
            Context.GetLogger().Info($"Closing {_trend} as {Sender.Path.ToString()} suggests; REVENUE IN THEORY: {revenueInTheory}");
            numberOfClosedBets++;

            if (numberOfClosedBets == numberOfClosingSimulators)
            {
                Context.GetLogger().Info($"All ({numberOfClosingSimulators}) closing simulators did the job");
                Self.Tell(PoisonPill.Instance);
            }
        });
    }
}