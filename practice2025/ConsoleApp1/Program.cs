using task10;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            PluginsLoader pluginsLoader = new PluginsLoader(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "plugins"));

            pluginsLoader.LoadPlugins();

        }
    }
}
