using Akka.Actor;
using Akka.Event;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Clients;
using SimpleTrader.Events;
using static System.String;
using static SimpleTrader.Exchange.Ticker;

namespace SimpleTrader.Exchange;

public class RealtimeKrakenWatcher : ReceiveActor
{
    public RealtimeKrakenWatcher(KrakenClient krakenRestClient, KrakenSocketClient krakenSocketClient, IActorRef receiver)
    {
        var lastUpdate = NullMarketUpdated.Instance;
        var self = Self;

        krakenSocketClient.SpotStreams.SubscribeToTickerUpdatesAsync($"{Crypto}/USD", data =>
            {
                var newUpdate = new MarketUpdated(data.Topic ?? Empty, data.Timestamp, data.Data.LastTrade.Price);
                if (!lastUpdate.Equals(newUpdate)) receiver.Tell(lastUpdate, self);
                lastUpdate = newUpdate;
            })
            .ContinueWith(r => r.Result)
            .PipeTo(self);

        Receive<CallResult<UpdateSubscription>>(r =>
        {
            Context.GetLogger().Info("Received websocket subscribe result");
            if (!r.Success)
                throw new Exception($"Failed to create subscription: {r.Error?.Message}");
        });
    }
}