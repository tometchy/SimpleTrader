using Akka.Actor;
using Infrastructure;
using SimpleTrader;
using CryptoExchange.Net.Authentication;
using Kraken.Net.Clients;
using Kraken.Net.Objects;
using SimpleTrader.Exchange;
using static System.String;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

string krakenApiKey = Environment.GetEnvironmentVariable("KRAKEN_API_KEY") ?? Empty;
string krakenApiKeySecret = Environment.GetEnvironmentVariable("KRAKEN_API_KEY_SECRET") ?? Empty;
ApiCredentials? apiCredentials = IsNullOrWhiteSpace(krakenApiKey) || IsNullOrWhiteSpace(krakenApiKeySecret)
        ? new ApiCredentials(krakenApiKey, krakenApiKeySecret)
        : null;

var krakenClient = new KrakenClientAdapter(new KrakenClient(new KrakenClientOptions
{
    ApiCredentials = apiCredentials,
    LogLevel = LogLevel.Trace,
    RequestTimeout = TimeSpan.FromSeconds(20)
}));

var krakenSocketClient = new KrakenSocketClient(new KrakenSocketClientOptions()
{
    ApiCredentials = apiCredentials,
    LogLevel = LogLevel.Trace,
});

new SyncAppProcess(new AppBridge(Props.Create(() => new App(krakenClient, krakenSocketClient))))
    .StartAndWaitForTermination();