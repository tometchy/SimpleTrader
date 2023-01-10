using Akka.Actor;
using Akka.Event;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Kraken.Net.Clients;
using static System.String;

namespace SimpleTrader.Exchange;

public class RealtimeKrakenWatcher : ReceiveActor
{
    public RealtimeKrakenWatcher(IExchangeClient krakenRestClient, KrakenSocketClient krakenSocketClient, IActorRef receiver)
    {
        krakenRestClient.GetTradablePairsTickers().ContinueWith(r => r.Result).PipeTo(Self);

        Receive<TradablePairsTickers>(symbols =>
        {
            krakenSocketClient.SpotStreams.SubscribeToTickerUpdatesAsync(symbols.PairsTickers, d =>
                    receiver.Tell(new NewTradeExecuted(d.Topic ?? Empty, d.Timestamp, d.Data.LastTrade.Price, d.Data.LastTrade.Quantity)))
                .ContinueWith(r => r.Result)
                .PipeTo(Self);
        });

        Receive<CallResult<UpdateSubscription>>(r =>
        {
            Context.GetLogger().Info("Received websocket subscribe result");
            if (!r.Success)
                throw new Exception($"Failed to create subscription: {r.Error?.Message}");
        });
    }
}