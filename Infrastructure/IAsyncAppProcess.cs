namespace Infrastructure
{
    public interface IAsyncAppProcess
    {
        void Start();
        Task TerminationHandle { get; }
        void ForceTermination();
    }
}