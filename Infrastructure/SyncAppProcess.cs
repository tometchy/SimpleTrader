using System;

namespace Infrastructure
{
    public class SyncAppProcess
    {
        private readonly IAsyncAppProcess _asyncAppProcess;

        public SyncAppProcess(IAsyncAppProcess asyncAppProcess) => _asyncAppProcess = asyncAppProcess;

        public void StartAndWaitForTermination()
        {
            try
            {
                _asyncAppProcess.Start();

                Console.CancelKeyPress += (sender, eventArgs) => _asyncAppProcess.ForceTermination();

                _asyncAppProcess.TerminationHandle.Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}