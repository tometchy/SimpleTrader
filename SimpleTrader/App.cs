using System;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using Kraken.Net.Clients;
using Kraken.Net.Objects;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace SimpleTrader
{
    public class App : ReceiveActor
    {
        public App()
        {
            Context.GetLogger().Info("Creating app");
            // Context.ActorOf(Props.Create(() => new UsersRepository()), nameof(UsersRepository));
            var krakenClient = new KrakenClient(new KrakenClientOptions()
            {
                // ApiCredentials = new ApiCredentials("API-KEY", "API-SECRET"),
                LogLevel = LogLevel.Trace,
                RequestTimeout = TimeSpan.FromSeconds(60)
            });
            
            // Getting ticker
            var tickerData = krakenClient.SpotApi.ExchangeData.GetTickerAsync("XBTUSD").Result;
            Console.WriteLine(tickerData.Data.First().Value.LastTrade.Price);
        }
    }
}