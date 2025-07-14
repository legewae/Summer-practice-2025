using task10;

namespace Amogus
{
    [LoadPlugin("Independant amogus", new string[] {})]
    public class Amogus:IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("Amogus plugin executed! Sus!");
        }
    }

    [LoadPlugin("Dependant amogus", new string[] {"Independant amogus"})]
    public class DependantAmogus : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("Dependant amogus initiated! Sus sus!");
        }
    }

    [LoadPlugin("Doubly dependant amogus", new string[] { "Dependant amogus" })]
    public class DoublyDependantAmogus:IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("Doubly dependant amogus initiated! Sus sus sus!");
        }
    }

    [LoadPlugin("Doublemogus", new string[] { "Dependant amogus", "Independant amogus" })]
    public class Doublemogus : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("Doublemogus initiated! sus^2!");
        }
    }

    [LoadPlugin("Externally dependant mogus", new string[] { "Dependant imposter" })]
    public class Externalmogus : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("Externalmogus is executed!");
        }
    }

    public class HomelessAmogus: IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("Homeless amogus is executed, which should be impossible...");
        }
    }
}
