using System;
using System.Reflection;
using System.Windows.Input;

namespace CommandRunner
{
    public static class Program
    {
        public static void Main()
        {
            var dll = Assembly.LoadFrom("FileSystemCommands.dll");

            var testDir = Path.Combine(Path.GetTempPath(), "TestDir2");
            Directory.CreateDirectory(testDir);
            File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
            File.WriteAllText(Path.Combine(testDir, "test2.log"), "World");

            foreach (var type in dll.GetTypes())
            {
                if (type.Name == "DirectorySizeCommand")
                {
                    var command = (DirectorySizeCommand)Activator.CreateInstance(type, testDir);
                    command.Execute();

                    Console.WriteLine(((DirectorySizeCommand)command).result);
                }
                else if (type.Name == "FindFilesCommand")
                {
                    var command = (FindFilesCommand)Activator.CreateInstance(type, testDir, "*.txt");
                    command.Execute();

                    var result = ((FindFilesCommand)command).result;

                    foreach(var f in result)
                    {
                        Console.WriteLine(f);
                    }
                }
            }
            Directory.Delete(testDir, true);
        }
    }
}
