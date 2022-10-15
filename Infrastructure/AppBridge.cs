using System.Reflection;
using Akka.Actor;

namespace Infrastructure
{
    public class AppBridge : IAsyncAppProcess
    {
        private readonly Props _appProps;
        private readonly ActorSystem _system;

        public AppBridge(Props appProps)
        {
            _appProps = appProps;
            _system = ActorSystem.Create(CreateActorSystemName());
        }

        public void Start() => _system.ActorOf(_appProps, "App");

        public void ForceTermination() => _system.Terminate();

        public Task TerminationHandle => _system.WhenTerminated;

        private string CreateActorSystemName() => Assembly.GetEntryAssembly()?.GetName().Name?.Replace(".", "") ??
                                                  throw new NullReferenceException();
    }
}