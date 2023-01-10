using Akka.Actor;
using Akka.Event;
using SimpleTrader.BetClosers;
using SimpleTrader.Commands;
using SimpleTrader.Events;
using SimpleTrader.Exchange;
using static System.TimeSpan;

namespace SimpleTrader.Bet;

public class Bet : ReceiveActor
{
    private readonly TrendDetected _trend;
    private readonly IExchangeReader _exchange;
    private ICancelable _scheduler;
    private readonly TimeSpan _stabilizePeriod = FromMinutes(1);

    public Bet(TrendDetected trend, IExchangeReader exchange)
    {
        _trend = trend;
        _exchange = exchange;
        Persist("Creating bet");
        Become(OpeningBet);
    }

    private void OpeningBet()
    {
        Persist($"Opening bet, stabilize time: {_stabilizePeriod}");
        Context.System.Scheduler.ScheduleTellOnce(_stabilizePeriod, Self, BetOpened.Instance, Self);
        // Exchange will be used somewhere here
        Receive<BetOpened>(_ => Become(Opened));
    }

    private void Opened()
    {
        Persist("Bet opened");
        EnsureDataAvailabilityEvenWithRealtimeUpdatesProblems();

        var numberOfClosingSimulators = 0;
        CreateClosingSimulator(Props.Create(() => new FixedPercentageBetCloser(_trend, 1)), nameof(FixedPercentageBetCloser) + "_1");
        CreateClosingSimulator(Props.Create(() => new FixedPercentageBetCloser(_trend, 0.5m)), nameof(FixedPercentageBetCloser) + "_0.5");
        CreateClosingSimulator(Props.Create(() => new FixedPercentageBetCloser(_trend, 1.5m)), nameof(FixedPercentageBetCloser) + "_1.5");

        Receive<NewTradeExecuted>(m =>
        {
            if(!string.Equals(m.PairTicker, _trend.PairTicker, StringComparison.InvariantCultureIgnoreCase))
                return;
            
            // In real scenarios stoploss defined in order will be used
            if (_trend.BetType == BetType.Long && m.LastTradePrice < _trend.LastPrice)
            {
                Context.GetLogger().Info($"{_trend.BetType} bet stop loss ({_trend.LastPrice}) crossed");
                Self.Tell(new CloseBet(m.LastTradePrice));
            }
            else if (_trend.BetType == BetType.Short && m.LastTradePrice > _trend.LastPrice)
            {
                Context.GetLogger().Info($"{_trend.BetType} bet stop loss ({_trend.LastPrice}) crossed");
                Self.Tell(new CloseBet(m.LastTradePrice));
            }
            else
                Context.ActorSelection("*").Tell(m, Sender);
        });

        Receive<CloseBet>(c =>
        {
            var revenueInTheory = _trend.BetType == BetType.Long
                ? $"{c.ClosingPrice * 100m / _trend.LastPrice - 100m}%"
                : $"{_trend.LastPrice * 100m / c.ClosingPrice - 100m}%";

            Persist($"Closing bet as {Sender.Path} suggests; REVENUE IN THEORY: {revenueInTheory}");
            RemoveClosingSimulator();

            if (CheckAllClosingSimulatorsAreRemoved()) Self.Tell(PoisonPill.Instance);
        });

        void CreateClosingSimulator(Props simulatorProps, string simulatorName)
        {
            Context.ActorOf(simulatorProps, simulatorName);
            numberOfClosingSimulators++;
        }

        void RemoveClosingSimulator()
        {
            Sender.Tell(PoisonPill.Instance);
            numberOfClosingSimulators--;
        }

        bool CheckAllClosingSimulatorsAreRemoved() => numberOfClosingSimulators == 0;

        void EnsureDataAvailabilityEvenWithRealtimeUpdatesProblems()
        {
            _scheduler = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(Zero, FromMinutes(1), Self, TimerElapsed.Instance, Self);
            Receive<TimerElapsed>(_ => _exchange.GetLastTradeData(_trend.PairTicker)
                .ContinueWith(r => new NewTradeExecuted(_trend.PairTicker, DateTime.UtcNow, r.Result.Price, r.Result.Quantity))
                .PipeTo(Self));
        }
    }

    private void Persist(string text)
    {
        var message = $"{DateTime.UtcNow} [{_trend}] >> {text}";
        Context.GetLogger().Info(message);
        File.AppendAllText($"/var/simple-trader/{_trend.Id}.txt", message + "\n");
    }

    protected override void PostStop()
    {
        _scheduler.Cancel();
        base.PostStop();
    }
}