    using System.Reflection;
    using System.Text;
    using task07;
    using Xunit;
    using ConsoleRunner;

    public class AttributeReflectionTests
    {

        [Fact]
        public void Class_HasDisplayNameAttribute()
        {
            var type = typeof(SampleClass);
            var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
            Assert.NotNull(attribute);
            Assert.Equal("Пример класса", attribute.DisplayName);
        }

        [Fact]
        public void Method_HasDisplayNameAttribute()
        {
            var method = typeof(SampleClass).GetMethod("TestMethod");
            var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
            Assert.NotNull(attribute);
            Assert.Equal("Тестовый метод", attribute.DisplayName);
        }

        [Fact]
        public void Property_HasDisplayNameAttribute()
        {
            var prop = typeof(SampleClass).GetProperty("Number");
            var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
            Assert.NotNull(attribute);
            Assert.Equal("Числовое свойство", attribute.DisplayName);
        }

        [Fact]
        public void Class_HasVersionAttribute()
        {
            var type = typeof(SampleClass);
            var attribute = type.GetCustomAttribute<VersionAttribute>();
            Assert.NotNull(attribute);
            Assert.Equal(1, attribute.Major);
            Assert.Equal(0, attribute.Minor);
        }

        [Fact]
        public void DLL_NoPathToLibrary()
        {

            var writer = new StringWriter();

            Console.SetOut(writer);

            ConsoleRunner.Program.Main(new string[] {});

            var output = writer.ToString();

            Assert.Contains("Передайте путь к библиотеке!", output);
    }

        [Fact]
        public void DLL_ChecksClasses()
        {

            var writer = new StringWriter();

            Console.SetOut(writer);

            ConsoleRunner.Program.Main(new string[] { "task07.dll" });

            var output = writer.ToString();

            Assert.Contains("Класс: SampleClass", output);
            Assert.Contains("Класс: DisplayNameAttribute", output);
            Assert.Contains("Класс: VersionAttribute", output);
            Assert.Contains("Класс: ReflectionHelper",output);
    }

        [Fact]
        public void DLL_ChecksMethods()
        {

            var writer = new StringWriter();

            Console.SetOut(writer);
        
            ConsoleRunner.Program.Main(new string[] { "task07.dll" });

            var output = writer.ToString();

            Assert.Contains("Метод TestMethod",output);
            Assert.Contains("Метод PrintTypeInfo",output);
        }

        [Fact]
        public void DLL_ContainsConstructors()
            {

            var writer = new StringWriter();

            Console.SetOut(writer);

            ConsoleRunner.Program.Main(new string[] { "task07.dll" });

            var output = writer.ToString();

            Assert.Contains("Конструктор", output);
        }
    [Fact]
    public void DLL_ContainsAttributes()
    {

        var writer = new StringWriter();

        Console.SetOut(writer);

        ConsoleRunner.Program.Main(new string[] { "task07.dll" });

        var output = writer.ToString();

        Assert.Contains("Атрибут:", output);
    }
}