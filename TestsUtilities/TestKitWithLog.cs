using Akka.Configuration;
using Akka.TestKit.NUnit3;
using Akka.Util.Internal;
using NLog;
using NLog.Targets;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace TestsUtilities;

public abstract class TestKitWithLog : TestKit
{
    private const string _loggerConfiguration = @"
            akka.loggers = [""Akka.Logger.NLog.NLogLogger, Akka.Logger.NLog""]
            akka.scheduler.implementation = ""Akka.TestKit.TestScheduler, Akka.TestKit""
            akka.loglevel = DEBUG";
        
    public TestKitWithLog(string config) 
        : base(ConfigurationFactory.ParseString(config).WithFallback(ConfigurationFactory.ParseString(_loggerConfiguration)))
    {
    }
        
    public TestKitWithLog() 
        : base(_loggerConfiguration)
    {
    }
        
    [AfterScenario]
    // [TearDown]
    // I don't know why both Console.WriteLine and TestContext.WriteLine works inside regular class, but not in Actors
    // so console output from NLog is not visible in test explorer. Maybe that is because of invocation from different thread?
    // That's why I was forced to use workaround with logging to memory collection first, then dumping it to output during tear down.
    public void TearDown()
    {
        var nlogMemoryTarget = LogManager.Configuration.FindTargetByName<MemoryTarget>("memory");
        var logsReadySignal = new ManualResetEvent(false);
        Sys.Terminate().Wait();
            
        Sys.WhenTerminated.ContinueWith(_ =>
        {
            nlogMemoryTarget.Flush(e =>
            {
                if(e != null) TestContext.WriteLine($"Exception during flushing logs: {e.Message}");

                nlogMemoryTarget.Logs.ToArray().ForEach(TestContext.WriteLine);
                nlogMemoryTarget.Logs.Clear();

                logsReadySignal.Set();
            });
        });

        logsReadySignal.WaitOne(TimeSpan.FromSeconds(1));
    }
}