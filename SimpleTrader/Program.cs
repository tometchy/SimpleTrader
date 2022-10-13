using Akka.Actor;
using Infrastructure;
using SimpleTrader;
using CryptoExchange.Net.Authentication;
using Kraken.Net.Clients;
using Kraken.Net.Objects;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

new SyncAppProcess(new AppBridge(Props.Create(() => new App(new KrakenClient(new KrakenClientOptions
{
    ApiCredentials = new ApiCredentials(
        Environment.GetEnvironmentVariable("KRAKEN_API_KEY")!,
        Environment.GetEnvironmentVariable("KRAKEN_API_KEY_SECRET")!),
    LogLevel = LogLevel.Trace,
    RequestTimeout = TimeSpan.FromSeconds(20)
}))))).StartAndWaitForTermination();