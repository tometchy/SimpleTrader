using Akka.Actor;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Clients;
using static System.String;
using static SimpleTrader.Ticker;

namespace SimpleTrader;

public class RealtimeMarketWatcher : ReceiveActor
{
    public RealtimeMarketWatcher(KrakenSocketClient kraken, IActorRef receiver)
    {
        var self = Self;
        kraken.SpotStreams.SubscribeToTickerUpdatesAsync($"{Crypto}/USD",
                data => receiver.Tell(new MarketUpdated(data.Topic ?? Empty, data.Timestamp, data.Data.LastTrade.Price), self))
            .ContinueWith(r => r.Result);

        Receive<CallResult<UpdateSubscription>>(r =>
        {
            if (!r.Success)
                throw new Exception($"Failed to create subscription: {r.Error?.Message}");
        });
    }
}