using Akka.Actor;
using Infrastructure;
using SimpleTrader;

var appProcess = new SyncAppProcess(new AppBridge(Props.Create(() => new App())));
appProcess.StartAndWaitForTermination();
