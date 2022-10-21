using System.Globalization;
using NUnit.Framework;
using SimpleTrader;
using TestsUtilities;
using static SimpleTrader.BetParameters;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

namespace SimpleTraderTests;

public class BetParametersCreationTests
{
    public class UserIdentityActorSpecs
    {
        [Test]
        public void Converting_parameters_should_work()
        {
            var id = F.Create<string>();
            var cryptoTicker = F.Create<string>();
            var betType = F.Create<BetType>();
            var initialPriceUsd = F.Create<decimal>();
            var threshold = F.Create<decimal>();
            var interval = F.CreateTimeSpanRoundedToSeconds();
            var amount = F.Create<decimal>();

            var bet = new BetParameters(id,
                cryptoTicker,
                betType.ToString(),
                initialPriceUsd.ToString(CultureInfo.InvariantCulture),
                threshold.ToString(CultureInfo.InvariantCulture),
                F.FormatTimeSpanToHoconFormat(interval),
                amount.ToString(CultureInfo.InvariantCulture));

            Assert.That(bet.CryptoTicker, Is.EqualTo(cryptoTicker));
            Assert.That(bet.Type, Is.EqualTo(betType));
            Assert.That(bet.InitialPriceUsd, Is.EqualTo(initialPriceUsd));
            Assert.That(bet.Threshold, Is.EqualTo(threshold));
            Assert.That(bet.PriceCheckInterval, Is.EqualTo(interval));
            Assert.That(bet.CryptoAmount, Is.EqualTo(amount));
        }

        [Test]
        public void Converting_parameters_should_fail_due_to_malformed_bet_type() =>
            Assert.Throws<ArgumentException>(() => new BetParameters(F.Create<string>(),
                F.Create<string>(),
                "MALFORMED",
                F.Create<decimal>().ToString(CultureInfo.InvariantCulture),
                F.Create<decimal>().ToString(CultureInfo.InvariantCulture),
                F.FormatTimeSpanToHoconFormat(F.CreateTimeSpanRoundedToSeconds()),
                F.Create<decimal>().ToString(CultureInfo.InvariantCulture)));
    }
}