using Xunit;
using task18;
using System.Runtime.CompilerServices;


public class LongCommand: ILongRunningCommand
{
    public int times = 3;
    public bool IsCompleted { get; set; } = false;

    public void Execute()
    {
        times--;
        if (times <= 0) IsCompleted = true; 
    }
}

public class ShortCommand: ICommand
{
    public bool IsCompleted { get; set; } = false;
    public void Execute()
    {
        IsCompleted = true;
    }
}

public class DelayedLongCommand : ILongRunningCommand
{
    public int times = 10;
    public bool IsCompleted { get; set; } = false;

    public void Execute()
    {
        Thread.Sleep(50);
        times--;
        if (times <= 0) IsCompleted = true;
    }
}
public class ServerThreadTests  
{
    [Fact]
    public void LongCommand_ShouldFinish()
    {
        var server = new ServerThread();

        var testCommand = new LongCommand();

        server.AddCommand(testCommand);
        Thread.Sleep(100);

        Assert.True(testCommand.IsCompleted);

        server.AddCommand(new HardStopCommand(server));
    }

    [Fact]
    public void LongCommand_ShouldFinishWithShortCommands()
    {
        var server = new ServerThread();

        var shortCommand1 = new ShortCommand();
        var shortCommand2 = new ShortCommand();
        var longCommand = new LongCommand();

        server.AddCommand(shortCommand1);
        server.AddCommand(longCommand);
        server.AddCommand(shortCommand2);

        Thread.Sleep(100);

        Assert.True(shortCommand1.IsCompleted);
        Assert.True(shortCommand2.IsCompleted);
        Assert.True(longCommand.IsCompleted);

        server.AddCommand(new HardStopCommand(server));
    }

    [Fact]
    public void LongCommand_ShouldFinishMultipleLongCommands()
    {
        var server = new ServerThread();

        var longCommand1 = new LongCommand();
        var longCommand2 = new LongCommand();
        var longCommand3 = new LongCommand();

        server.AddCommand(longCommand1);
        server.AddCommand(longCommand2);
        server.AddCommand(longCommand3);

        Thread.Sleep(100);

        Assert.True(longCommand1.IsCompleted);
        Assert.True(longCommand2.IsCompleted);
        Assert.True(longCommand3.IsCompleted);

        server.AddCommand(new HardStopCommand(server));
    }

    [Fact]
    public void LongCommand_ShouldNotStopShortCommandsFromExecuting()
    {
        var server = new ServerThread();

        var longCommand = new DelayedLongCommand();
        var shortCommand = new ShortCommand();

        server.AddCommand(longCommand);
        server.AddCommand(shortCommand);

        Thread.Sleep(100);

        Assert.False(longCommand.IsCompleted);
        Assert.True(shortCommand.IsCompleted);


        server.AddCommand(new HardStopCommand(server));
    }
}
