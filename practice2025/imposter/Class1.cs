using task10;

[LoadPlugin("SecondLibrarySus", new string[] {})]
public class SecondLibrarySus : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("SecondLibrary class initiated!");
    }
}


[LoadPlugin("Dependant imposter", new string[] {"SecondLibrarySus"})]
public class Imposter : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Dependancy imposter is loaded!");
    }
}

[LoadPlugin("AllDependantPlugin", new string[] { "Dependant imposter", "Doublemogus", "Externally dependant mogus" })]
public class AllDependantPlugin : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Final plugin has loaded!");
    }
}