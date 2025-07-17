using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace task18
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ILongRunningCommand : ICommand
    {
        bool IsCompleted { get; set; }
    }

    public interface IScheduler
    {
        bool HasCommand();
        ICommand Select();
        void Add(ICommand cmd);
    }

    public class RoundRobinScheduler : IScheduler
    {
        private readonly Queue<ICommand> _commands = new Queue<ICommand>();
       

        public void Add(ICommand command)
        {
            _commands.Enqueue(command);
        }

        public bool HasCommand()
        {
            return _commands.Count > 0;
        }

        public ICommand Select()
        {
            if (_commands.Count == 0) return null;

            var command = (ILongRunningCommand)_commands.Dequeue();

            if (!command.IsCompleted)
            {
                _commands.Enqueue(command);
            }
            return command;
        }
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
        private readonly ConcurrentQueue<ICommand> _commandQueue = new ConcurrentQueue<ICommand>();
        private readonly Thread _workerThread;
        private readonly AutoResetEvent _commandAvailable = new AutoResetEvent(false);
        private readonly IScheduler _scheduler = new RoundRobinScheduler();
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

            if (command is ILongRunningCommand)
            {
                _scheduler.Add(command);
                _commandAvailable.Set();
            }
            else
            {
                _commandQueue.Enqueue(command);
                _commandAvailable.Set();
            }
        }

        internal void HardStop()
        {
            _hardStopRequested = true;
            _commandAvailable.Set();
        }

        internal void SoftStop()
        {
            _softStopRequested = true;
            _commandAvailable.Set();
        }

        private void ProcessCommands()
        {
            while (!_hardStopRequested)
            {
                if (_commandQueue.TryDequeue(out var command))
                {
                    ExecuteCommand(command);
                    continue;
                }

                if (_scheduler.HasCommand())
                {
                    var longRunningCommand = _scheduler.Select();
                    if (longRunningCommand != null)
                    {
                        ExecuteCommand(longRunningCommand);
                        continue;
                    }
                }

                if (_softStopRequested && !_scheduler.HasCommand())
                {
                    return;
                }

                _commandAvailable.WaitOne();
                _commandAvailable.Reset();
            }
        }

        private void ExecuteCommand(ICommand command)
        {
            try
            {
                command.Execute();

                if (command is ILongRunningCommand)
                {
                    if(!((ILongRunningCommand)command).IsCompleted)
                    _scheduler.Add(command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"We need a error handler real fast!!!");
            }
        }
    }
}