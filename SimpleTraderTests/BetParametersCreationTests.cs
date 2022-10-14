using System.Globalization;
using NUnit.Framework;
// ReSharper disable ClassNeverInstantiated.Global

namespace SimpleTraderTests;

public class BetParametersCreationTests
{
    public class UserIdentityActorSpecs
    {
        [Test]
        public void Converting_parameters_should_work()
        {
            var cryptoTicker = "btc";
            var betType = BetParameters.BetType.Long;
            decimal initialPriceUsd = 100000;
            double threshold = 5;
            double amount = 10000;

            var bet = new BetParameters(
                cryptoTicker,
                betType.ToString(),
                initialPriceUsd.ToString(CultureInfo.InvariantCulture),
                threshold.ToString(CultureInfo.InvariantCulture),
                amount.ToString(CultureInfo.InvariantCulture));

            Assert.That(bet.CryptoTicker, Is.EqualTo(cryptoTicker));
            Assert.That(bet.Type, Is.EqualTo(betType));
            Assert.That(bet.InitialPriceUsd, Is.EqualTo(initialPriceUsd));
            Assert.That(bet.Threshold, Is.EqualTo(threshold));
            Assert.That(bet.Amount, Is.EqualTo(amount));
        }

        [Test]
        public void Converting_parameters_should_fail_due_to_malformed_bet_type() =>
            Assert.Throws<Exception>(
                () => new BetParameters(
                    F.Create<string>(),
                    "MALFORMED",
                    F.Create<decimal>().ToString(CultureInfo.InvariantCulture),
                    F.Create<double>().ToString(CultureInfo.InvariantCulture),
                    F.Create<double>().ToString(CultureInfo.InvariantCulture)
                ));

        [TestCase(BetParameters.BetType.Short, 100.0)]
        [TestCase(BetParameters.BetType.Long, 0.5)]
        public void Converting_parameters_should_fail_due_to_long_with_short_confusion(
            BetParameters.BetType confusedBetType,
            decimal amount)
        {
            Assert.Throws<Exception>(
                () => new BetParameters(
                    F.Create<string>(),
                    confusedBetType.ToString(),
                    F.Create<decimal>().ToString(CultureInfo.InvariantCulture),
                    F.Create<double>().ToString(CultureInfo.InvariantCulture),
                    amount.ToString(CultureInfo.InvariantCulture)
                ));
        }
    }
}