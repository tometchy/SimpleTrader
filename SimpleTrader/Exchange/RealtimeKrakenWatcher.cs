using Akka.Actor;
using Akka.Event;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Clients;
using Kraken.Net.Objects.Models;
using SimpleTrader.Events;
using static System.String;
using static System.StringComparison;

namespace SimpleTrader.Exchange;

public class RealtimeKrakenWatcher : ReceiveActor
{
    public RealtimeKrakenWatcher(KrakenClient krakenRestClient, KrakenSocketClient krakenSocketClient, IActorRef receiver)
    {
        var self = Self;

        krakenRestClient.SpotApi.ExchangeData.GetSymbolsAsync().ContinueWith(r => r.Result).PipeTo(self);

        Receive<WebCallResult<Dictionary<string, KrakenSymbol>>>(r =>
        {
            var tickersToSubscribe = new List<string>(r.Data.Values.Select(symbol => symbol.WebsocketName)
                .Where(n => n.EndsWith("/USD") && !n.Contains("usdt", InvariantCultureIgnoreCase)));

            krakenSocketClient.SpotStreams.SubscribeToTickerUpdatesAsync(tickersToSubscribe, data => receiver.Tell(
                    new NewTradeExecuted(data.Topic ?? Empty, data.Timestamp, data.Data.LastTrade.Price, data.Data.LastTrade.Quantity),
                    self))
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