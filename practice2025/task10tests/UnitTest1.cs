using task10;

namespace task10tests
{
    public class UnitTest1
    {
        [Fact]
        public void Libraries_NoLibrariesDetected()
        {
            var writer = new StringWriter();
            Console.SetOut(writer);

            PluginsLoader pluginsLoader = new PluginsLoader("libraries");

            string output = writer.ToString();

            Assert.Contains("Директории не существует!", output);

            writer.Dispose();
            Console.SetOut(Console.Out);
        }

        [Fact]
        public void Libraries_LoadsAllPlugins()
        {
            var writer = new StringWriter();
            Console.SetOut(writer);

            PluginsLoader pluginsLoader = new PluginsLoader(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,"plugins"));

            pluginsLoader.LoadPlugins();

            string output = writer.ToString();
            Assert.Contains("Final plugin has loaded!", output); //Плагин, зависимый от всех, загружен успешно

            writer.Dispose();
            Console.SetOut(Console.Out);
        }
    }
}