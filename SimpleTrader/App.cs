using System;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using Kraken.Net.Clients;

namespace SimpleTrader
{
    public class App : ReceiveActor
    {
        public App(KrakenClient krakenClient)
        {
            Context.GetLogger().Info("Creating app");
            // Context.ActorOf(Props.Create(() => new UsersRepository()), nameof(UsersRepository));
            
            var tickerData = krakenClient.SpotApi.ExchangeData.GetTickerAsync("XBTUSD").Result;
            Console.WriteLine(tickerData.Data.First().Value.LastTrade.Price);
        }
    }
}