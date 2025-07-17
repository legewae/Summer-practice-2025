using System;
using System.Reflection;

namespace task07
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DisplayNameAttribute: Attribute
    {
        public string DisplayName;
        public DisplayNameAttribute(string name)
        {
            DisplayName = name;
        }
    }
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class VersionAttribute : Attribute
    {
        public int Major;
        public int Minor;
        public VersionAttribute(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }
    }

    [DisplayNameAttribute("Пример класса"), VersionAttribute(1,0)]
    public class SampleClass
    {
        [DisplayNameAttribute("Числовое свойство")]
        public int Number { get; set; }


        [DisplayNameAttribute("Тестовый метод")]
        public void TestMethod() { }

    }

    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            if(type == null) { return; }

            Console.WriteLine($"Класс: {type.Name}");
            
            var displayNameAtt = type.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAtt != null)
            {
                Console.WriteLine($"Отображаемое имя класса: {displayNameAtt.DisplayName}");
            }

            var versionAtt = type.GetCustomAttribute<VersionAttribute>();
            if (versionAtt != null)
            {
                Console.WriteLine($"Версия класса: {versionAtt.Major}.{versionAtt.Minor}");    
            }

            Console.WriteLine($"Свойства класса:");
            var properties = type.GetProperties();
            foreach(var property in properties)
            {
                var propDisplayNameAtt = property.GetCustomAttribute<DisplayNameAttribute>();
                if (propDisplayNameAtt != null)
                {
                    Console.WriteLine($"  {property.Name}: {propDisplayNameAtt.DisplayName}");
                }
                else
                {
                    Console.WriteLine($"  {property.Name}: Нету DisplayNameAttribute");
                }
            }

            Console.WriteLine($"Методы класса:");
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                var methodDisplayNameAtt = method.GetCustomAttribute<DisplayNameAttribute>();
                if (methodDisplayNameAtt != null)
                {
                    Console.WriteLine($"  {method.Name}: {methodDisplayNameAtt.DisplayName}");
                }
                else
                {
                    Console.WriteLine($"  {method.Name}: Нету DisplayNameAttribute");
                }
            }
        }
    }
}
