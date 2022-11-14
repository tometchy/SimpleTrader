using Akka.Actor;
using Akka.Event;
using SimpleTrader.Events;
using static System.TimeSpan;
using static SimpleTrader.BetType;
using static SimpleTrader.Ticker;

namespace SimpleTrader
{
    public static class Ticker
    {
        public const string Crypto = "SOL";
        public const string Stable = "USDC";
    }
    
    public class App : ReceiveActor
    {
        public App(IExchangeReader priceReader, IExchangeOrderMakerBridge orderMaker)
        {
            Context.ActorOf(Props.Create(() => new TrendDetector()), nameof(TrendDetector));

            Context.System.Scheduler.ScheduleTellRepeatedly(Zero, FromMinutes(1), Self, TimerElapsed.Instance, Self);
            Receive<TimerElapsed>(_ => priceReader.GetAssetPrice($"{Crypto}USD").ContinueWith(r => r.Result).PipeTo(Context.ActorSelection("*")));

            Receive<TrendDetected>(trend => Context.ActorOf(Props.Create(() => new Bet(orderMaker, trend.BetType, trend.LastPrice))));
        }
    }

    public class Bet : ReceiveActor
    {
        public Bet(IExchangeOrderMakerBridge exchange, BetType betType, decimal initialPrice)
        {
            // IBetOrder betOrder = NullBetOrder;
            if (betType == Long)
            {
                Context.GetLogger().Info($"Creating {betType} BET from initial price: {initialPrice}, BUYING IMMEDIATELY");
                // betOrder = exchange.BuyImmediately(Crypto, Stable); // TODO ERROR HANDLING
            }
            else
            {
                Context.GetLogger().Info($"Creating {betType} BET from initial price: {initialPrice}, SELLING IMMEDIATELY");
                // betOrder = exchange.SellImmediately(Crypto, Stable); // TODO ERROR HANDLING
                // TODO: Different LONG ticker not supported currently due to buy with all simplification
            }
                    
            Receive<decimal>(newPrice =>
            {
                Context.GetLogger().Info($"Received new price: {newPrice}");

                if (betType == Long && newPrice < initialPrice)
                {
                    Context.GetLogger().Info($"{betType} bet stop loss ({initialPrice}) crossed, closing - SELLING IMMEDIATELY");
                    exchange.SellImmediately(Crypto, Stable);
                }

                if (betType == Short && newPrice > initialPrice)
                {
                    Context.GetLogger().Info($"{betType} bet stop loss ({initialPrice}) crossed, closing - BUYING IMMEDIATELY");
                    exchange.BuyImmediately("SOL", "USDC");
                }
            });
        }
    }


    public class TrendDetector : ReceiveActor
    {
        readonly List<decimal> _pricesAtMinInterval = new();

        public TrendDetector()
        {
            Receive<decimal>(newPrice =>
            {
                Context.GetLogger().Info($"Received new price: {newPrice}");
                _pricesAtMinInterval.Add(newPrice);

                if (_pricesAtMinInterval.Count < 15)
                {
                    Context.GetLogger().Info($"Not enough prices at min interval ({_pricesAtMinInterval.Count})");
                    return;
                }

                if (_pricesAtMinInterval[^1] > ((_pricesAtMinInterval[^2]/100)*101) ||
                    _pricesAtMinInterval[^1] > ((_pricesAtMinInterval[^3]/100)*101) || 
                    _pricesAtMinInterval[^1] > ((_pricesAtMinInterval[^4]/100)*101) ||
                    _pricesAtMinInterval[^1] > ((_pricesAtMinInterval[^5]/100)*101) ||
                    _pricesAtMinInterval[^1] > ((_pricesAtMinInterval[^6]/100)*101))
                {
                    Context.GetLogger().Info($"LONG Bet detected from prices: {string.Join(", ", _pricesAtMinInterval)}");
                    Context.Parent.Tell(new TrendDetected(Long, _pricesAtMinInterval.Last()));
                }

                else if (_pricesAtMinInterval[^1] < ((_pricesAtMinInterval[^2]/100)*99) ||
                    _pricesAtMinInterval[^1] < ((_pricesAtMinInterval[^3]/100)*99)  || 
                    _pricesAtMinInterval[^1] < ((_pricesAtMinInterval[^4]/100)*99) ||
                    _pricesAtMinInterval[^1] < ((_pricesAtMinInterval[^5]/100)*99) ||
                    _pricesAtMinInterval[^1] < ((_pricesAtMinInterval[^6]/100)*99))
                {
                    Context.GetLogger().Info($"SHORT Bet detected from prices: {string.Join(", ", _pricesAtMinInterval)}");
                    Context.Parent.Tell(new TrendDetected(Short, _pricesAtMinInterval.Last()));
                }
            });
        }
    }
}