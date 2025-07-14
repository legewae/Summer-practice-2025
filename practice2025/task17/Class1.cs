using System.Collections.Concurrent;
using System.Threading;

namespace task17
{
    public interface ICommand
    {
        void Execute();
    }

    public class HardStopCommand : ICommand
    {
        private ServerThread _serverThread;

        public HardStopCommand(ServerThread serverThread)
        {
            _serverThread = serverThread;
        }

        public void Execute()
        {
            if (Thread.CurrentThread != _serverThread.WorkerThread)
                throw new Exception("HardStop can only be executed in its own thread!");

            _serverThread.HardStop();
        }
    }

    public class SoftStopCommand : ICommand
    {
        private ServerThread _serverThread;

        public SoftStopCommand(ServerThread serverThread)
        {
            _serverThread = serverThread;
        }

        public void Execute()
        {
            if (Thread.CurrentThread != _serverThread.WorkerThread)
                throw new Exception("SoftStop can only be executed in its own thread!");

            _serverThread.SoftStop();
        }
    }

    public class ServerThread
    {
        private ConcurrentQueue<ICommand> _commandQueue = new();
        private Thread _workerThread;
        private AutoResetEvent _commandAvailable = new(false);
        private volatile bool _hardStopRequested = false;
        private volatile bool _softStopRequested = false;

        public Thread WorkerThread => _workerThread;

        public ServerThread()
        {
            _workerThread = new Thread(ProcessCommands);
            _workerThread.Start();
        }

        public void AddCommand(ICommand command)
        {
            if (_hardStopRequested) return;
            _commandQueue.Enqueue(command);
            _commandAvailable.Set();
        }

        internal void HardStop()
        {
            _hardStopRequested = true;
            _commandAvailable.Set();
        }

        internal void SoftStop()
        {
            _softStopRequested = true;
        }

        private void ProcessCommands()
        {
            while (!_hardStopRequested)
            {
                if (_commandQueue.TryDequeue(out var command))
                {
                    try
                    {
                        command.Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Command failed with error: {ex.Message}, exception handler should be ");
                    }
                }
                else if (_softStopRequested)
                {
                    return;
                }
                else
                {
                    _commandAvailable.WaitOne();
                    _commandAvailable.Reset();
                }
            }
        }
    }
}