using System;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using Kraken.Net.Clients;

namespace SimpleTrader
{
    public class App : ReceiveActor
    {
        public App(IKrakenClientAdapter krakenClient, BetParameters bet)
        {
            Context.GetLogger().Info("Creating app");
            // Context.ActorOf(Props.Create(() => new UsersRepository()), nameof(UsersRepository));

            var assetPrice = krakenClient.GetAssetPrice("XBTUSD");
            Console.WriteLine(assetPrice);
        }
    }
}