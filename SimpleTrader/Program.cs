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

var krakenClientOptions = IsNullOrWhiteSpace(krakenApiKey) || IsNullOrWhiteSpace(krakenApiKeySecret)
    ? new KrakenClientOptions { LogLevel = LogLevel.Trace, RequestTimeout = TimeSpan.FromSeconds(20) }
    : new KrakenClientOptions
    {
        ApiCredentials = new ApiCredentials(krakenApiKey, krakenApiKeySecret),
        LogLevel = LogLevel.Trace,
        RequestTimeout = TimeSpan.FromSeconds(20)
    };

KrakenClient krakenRestClient = new KrakenClient(krakenClientOptions);

var krakenSocketClient = new KrakenSocketClient(new KrakenSocketClientOptions()
{
        ApiCredentials = new ApiCredentials(krakenApiKey, krakenApiKeySecret),
        LogLevel = LogLevel.Trace,
});

var krakenAdapter = new KrakenAdapter(krakenRestClient);

new SyncAppProcess(new AppBridge(Props.Create(() =>
        new App(krakenRestClient, krakenSocketClient, krakenAdapter, new ExchangeBridge(krakenAdapter)))))
    .StartAndWaitForTermination();