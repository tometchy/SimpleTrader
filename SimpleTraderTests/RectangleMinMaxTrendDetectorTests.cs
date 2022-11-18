using Akka.Actor;
using NUnit.Framework;
using SimpleTrader.Events;
using SimpleTrader.TrendDetectors;
using TestsUtilities;
using static System.TimeSpan;

namespace SimpleTraderTests;

public class RectangleMinMaxTrendDetectorTests : TestKitWithLog
{
    [TestCase(60, 60, 11.0)]
    public void KrakenSolUsd1M221117_12_06(int howLongToLookBackMinutes, int howLongToSkipLastPricesFromRectangleMinutes,
        decimal howManyTimesRectangleMultiplied)
    {
        var detector = Sys.ActorOf(Props.Create(() => new RectangleMinMaxTrendDetector(FromMinutes(howLongToLookBackMinutes),
            FromMinutes(howLongToSkipLastPricesFromRectangleMinutes), howManyTimesRectangleMultiplied)));
        
        detector.Tell(new MarketUpdated("SOL/USD", DateTime.Parse("11/18/2022 22:09:08"), 13.18000m));

        Within(TimeSpan.FromMilliseconds(300), () => ExpectMsg<TrendDetected>());
    }
}