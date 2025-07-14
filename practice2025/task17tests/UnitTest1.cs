using Xunit;
using task17;
using System.Runtime.CompilerServices;

public class command1: ICommand{
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
public class ServerThreadTests  
{
    [Fact]
    public void ExecuteCommand_ShouldRunCommand()
    {
        var server = new ServerThread();

        var testCommand = new command1();

        server.AddCommand(testCommand);
        Thread.Sleep(100);

        Assert.True(testCommand.commandExecuted);

        server.AddCommand(new HardStopCommand(server));
    }

    [Fact]
    public void ExecuteCommand_ShouldRunMultipleCommands()
    {
        var server = new ServerThread();

        var testCommand1 = new command1();
        var testCommand2 = new command2();
        var testCommand3 = new command3();

        server.AddCommand(testCommand1);
        server.AddCommand(testCommand2);
        server.AddCommand(testCommand3);
        Thread.Sleep(2000);

        Assert.True(testCommand1.commandExecuted);
        Assert.True(testCommand2.commandExecuted);
        Assert.True(testCommand3.commandExecuted);

        server.AddCommand(new HardStopCommand(server));
    }

    [Fact]
    public void ExecuteCommand_SoftStopPreventsFromExecution()
    {
        var server = new ServerThread();

        var testCommand1 = new command1();
        var testCommand3 = new command3();

        server.AddCommand(testCommand1);
        server.AddCommand(new SoftStopCommand(server));
        Thread.Sleep(100);
        server.AddCommand(testCommand3);

        Assert.True(testCommand1.commandExecuted);
        Assert.False(testCommand3.commandExecuted);
    }

    [Fact]
    public void ExecuteCommand_LongCommandGivesEnoughTimeToSkipSoftStop()
    {
        var server = new ServerThread();

        var testCommand1 = new command1();
        var longCommand = new command3();

        server.AddCommand(longCommand);
        server.AddCommand(new SoftStopCommand(server));
        server.AddCommand(testCommand1);

        Thread.Sleep(2000);

        Assert.True(testCommand1.commandExecuted);
        Assert.True(longCommand.commandExecuted);
    }

    [Fact]
    public void ExecuteCommand_HardStopPreventsFurtherCommands()
    {
        var server = new ServerThread();

        var testCommand1 = new command1();
        var longCommand = new command3();

        server.AddCommand(longCommand);
        server.AddCommand(new HardStopCommand(server));
        server.AddCommand(testCommand1);

        Thread.Sleep(2000);

        Assert.False(testCommand1.commandExecuted);
        Assert.True(longCommand.commandExecuted);
    }
}
