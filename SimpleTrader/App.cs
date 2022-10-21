using Akka.Actor;
using Akka.Event;
using static System.TimeSpan;
using static SimpleTrader.BetParameters.BetType;

namespace SimpleTrader
{
    public class App : ReceiveActor
    {
        public App(IKrakenClientAdapter krakenClient, BetParameters bet)
        {
            Context.GetLogger().Info("Creating app");
            Context.System.Scheduler.ScheduleTellRepeatedly(Zero, bet.PriceCheckInterval, Self,
                IntervalElapsed.Instance, Self);

            var theBiggestObservedPrice = bet.InitialPriceUsd;
            var theSmallestObservedPrice = bet.InitialPriceUsd;

            Receive<IntervalElapsed>(_ =>
            {
                Context.GetLogger().Info($"{bet.PriceCheckInterval} interval elapsed, checking {bet.CryptoTicker}");
                krakenClient.GetAssetPrice($"{bet.CryptoTicker}USD")
                    .ContinueWith(r => r.Result)
                    .PipeTo(Self);
            });

            Receive<decimal>(currentPrice =>
            {
                Context.GetLogger().Info($"Received current price: {currentPrice} USD");
                if (currentPrice > theBiggestObservedPrice)
                {
                    Context.GetLogger().Info($"Bigger price than previous the biggest: ({theBiggestObservedPrice})");
                    theBiggestObservedPrice = currentPrice;
                }
                else if (currentPrice < theSmallestObservedPrice)
                {
                    Context.GetLogger().Info($"Smaller price than previous the smallest: ({theBiggestObservedPrice})");
                    theSmallestObservedPrice = currentPrice;
                }

                var longThresholdPrice = theBiggestObservedPrice - theBiggestObservedPrice * bet.Threshold;
                if (bet.Type == Long && currentPrice <= longThresholdPrice)
                {
                    Context.GetLogger()
                        .Info($"Price SMALLER than LONG threshold: ({longThresholdPrice}), " +
                              $"publishing {bet.CryptoAmount} {bet.CryptoTicker} LIMIT SELL order at {longThresholdPrice} price");
                    krakenClient.PublishLimitSellOrder(bet.CryptoTicker, bet.CryptoAmount, longThresholdPrice);
                }

                var shortThresholdPrice = theSmallestObservedPrice + theSmallestObservedPrice * bet.Threshold;
                if (bet.Type == Short && currentPrice >= shortThresholdPrice)
                {
                    Context.GetLogger()
                        .Info($"Price BIGGER than SHORT threshold: ({shortThresholdPrice}), " +
                              $"publishing {bet.CryptoAmount} {bet.CryptoTicker} LIMIT BUY order at {shortThresholdPrice} price");
                    krakenClient.PublishLimitBuyOrder(bet.CryptoTicker, bet.CryptoAmount, shortThresholdPrice);
                }
            });

            // Context.ActorOf(Props.Create(() => new UsersRepository()), nameof(UsersRepository));
        }

        public class IntervalElapsed
        {
            public static IntervalElapsed Instance { get; } = new();

            private IntervalElapsed()
            {
            }
        }
    }
}