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
    public RealtimeKrakenWatcher(KrakenSocketClient kraken, IActorRef receiver)
    {
        var lastUpdate = NullMarketUpdated.Instance;
        var self = Self;

        kraken.SpotStreams.SubscribeToTickerUpdatesAsync($"{Crypto}/USD",
                data =>
                {
                    var isTheSamePriceAtTheSameTime = data.Data.LastTrade.Price == lastUpdate.LastTradePrice &&
                                                      TrimMilliseconds(data.Timestamp) == TrimMilliseconds(lastUpdate.Timestamp);
                    lastUpdate = new MarketUpdated(data.Topic ?? Empty, data.Timestamp, data.Data.LastTrade.Price);
                    if (!isTheSamePriceAtTheSameTime) receiver.Tell(lastUpdate, self);
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

    private static DateTime TrimMilliseconds(DateTime dt) => new(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);
}