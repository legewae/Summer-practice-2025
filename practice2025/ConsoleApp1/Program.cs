using task17;

namespace ConsoleApp1
{
    internal class Program
    {
        public class command1 : ICommand
        {
            public bool commandExecuted = false;
            public void Execute()
            {
                Console.WriteLine("Command1 executed");
                commandExecuted = true;
            }
        }

        public class command2 : ICommand
        {
            public bool commandExecuted = false;
            public void Execute()
            {
                Console.WriteLine("Command2 executed");
                commandExecuted = true;
            }
        }

        public class command3 : ICommand
        {
            public bool commandExecuted = false;
            public void Execute()
            {
                Console.WriteLine("Command3 executed");
                commandExecuted = true;
            }
        }

        public class longCommand : ICommand
        {
            public bool commandExecuted = false;
            public void Execute()
            {
                Thread.Sleep(1000);
                Console.WriteLine("Long command executed");
                commandExecuted = true;
            }
        }
        static void Main(string[] args)
        {
            var server = new ServerThread();
            var testCommand1 = new command1();
            var longCommand = new command3();

            server.AddCommand(longCommand);
            server.AddCommand(new SoftStopCommand(server));
            server.AddCommand(testCommand1);
        }
    }
}
