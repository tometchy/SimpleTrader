using Akka.Actor;
using Akka.Event;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Clients;
using Kraken.Net.Objects.Models;
using SimpleTrader.Events;
using static System.String;

namespace SimpleTrader.Exchange;

public class RealtimeKrakenWatcher : ReceiveActor
{
    public RealtimeKrakenWatcher(KrakenClient krakenRestClient, KrakenSocketClient krakenSocketClient, IActorRef receiver)
    {
        var self = Self;

        krakenRestClient.SpotApi.ExchangeData.GetSymbolsAsync().ContinueWith(r => r.Result).PipeTo(self);

        Receive<WebCallResult<Dictionary<string, KrakenSymbol>>>(r =>
        {
            var tickersToSubscribe = new List<string>(r.Data.Values.Select(symbol => symbol.WebsocketName).Where(n => n.EndsWith("/USD")));
            var lastUpdate = NullMarketUpdated.Instance;
            krakenSocketClient.SpotStreams.SubscribeToTickerUpdatesAsync(tickersToSubscribe, data =>
                {
                    var newUpdate = new MarketUpdated(data.Topic ?? Empty, data.Timestamp, data.Data.LastTrade.Price);
                    if (!lastUpdate.Equals(newUpdate)) receiver.Tell(newUpdate, self);
                    lastUpdate = newUpdate;
                })
                .ContinueWith(r => r.Result)
                .PipeTo(self);
        });


        Receive<CallResult<UpdateSubscription>>(r =>
        {
            Context.GetLogger().Info("Received websocket subscribe result");
            if (!r.Success)
                throw new Exception($"Failed to create subscription: {r.Error?.Message}");
        });
    }
}