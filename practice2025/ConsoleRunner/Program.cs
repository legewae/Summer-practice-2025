using System;
using System.Reflection;

namespace ConsoleRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Передайте путь к библиотеке!");
                return;
            }

            string assemblyPath = args[0];
            if (!System.IO.File.Exists(assemblyPath))
            {
                Console.WriteLine("Файл не найден: " + assemblyPath);
                return;
            }

            var assembly = Assembly.LoadFrom(assemblyPath);

            Console.WriteLine($"Динамическая библиотека {assembly.FullName}");
            foreach(var type in assembly.GetTypes())
            {
                Console.WriteLine($"Класс: {type.Name}");

                Console.WriteLine("Методы класса:");
                foreach(var method in type.GetMethods())
                {
                    Console.WriteLine($"Метод {method.Name}");
                    Console.WriteLine("Параметры метода:");
                    foreach(var par in method.GetParameters())
                    {
                        Console.WriteLine($"{par.Name} типа {par.ParameterType.Name}");
                    }
                }

                Console.WriteLine("Свойства класса:");
                foreach (var prop in type.GetProperties())
                {
                    Console.WriteLine($"Свойство {prop.Name} типа {prop.PropertyType.Name}");
                }

                Console.WriteLine("Конструкторы класса:");
                foreach(var constructor in type.GetConstructors())
                {
                    Console.WriteLine($"Конструктор {constructor.Name}");
                    Console.WriteLine("Параметры конструктора:");
                    foreach (var par in constructor.GetParameters())
                    {
                        Console.WriteLine($"{par.Name} типа {par.ParameterType.Name}");
                    }
                }

                Console.WriteLine("Аттрибуты класса:");
                foreach(var att in type.GetCustomAttributes())
                {
                    Console.WriteLine($"Атрибут: {att.GetType().Name}");
                }
                
            }
        }
    }
}
