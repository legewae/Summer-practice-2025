using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace task10
{

    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class LoadPlugin : System.Attribute
    {
        public string Name { get; }
        public string[] Dependancies { get; }
        public LoadPlugin(string name, string[] dependancies)
        {
            Name = name;
            Dependancies = dependancies;
        }
    }

    public interface IPlugin
    {
        void Execute();
    }
    public class PluginsLoader
    {
        private string _path;

        public PluginsLoader(string pluginPath)
        {
            _path = pluginPath;

            if (!System.IO.Directory.Exists(_path))
            {
                Console.WriteLine("Директории не существует!");
            }
        }

        public void InitPlugin(Dictionary<string, Type> availablePlugins, Stack<string> loadedPlugins, string pluginName)
        {
            void LoadWithDependencies(string plugin)
            {
                if (loadedPlugins.Contains(plugin))
                {
                    return;
                }

                if (!availablePlugins.ContainsKey(plugin))
                {
                    Console.WriteLine($"Плагин {plugin} не найден, но требуется как зависимость");
                    return;
                }

                var pluginType = availablePlugins[plugin];
                var attr = (LoadPlugin)pluginType.GetCustomAttribute(typeof(LoadPlugin), false);
                if (attr == null) return;
                foreach (string dependency in attr.Dependancies)
                {
                    LoadWithDependencies(dependency);
                }

                var instance = (IPlugin)Activator.CreateInstance(pluginType);
                instance.Execute();
                Console.WriteLine($"Плагин {attr.Name} загружен и выполнен");

                loadedPlugins.Push(plugin);
            }
            LoadWithDependencies(pluginName);
        }

        public void LoadPlugins()
        {
            var files = System.IO.Directory.GetFiles(_path, "*.dll");

            Console.WriteLine($"Найдено библиотек: {files.Length}");

            Assembly[] assemblies = new Assembly[files.Length];
            int count = 0;
            foreach (var file in files)
            {
                if (AppDomain.CurrentDomain.GetAssemblies().Any(a => a.Location == file)) continue;
                var assembly = Assembly.LoadFrom(file);
                assemblies[count] = assembly;
                count++;
                Console.WriteLine(assembly.FullName);
            }

            Dictionary<string, Type> types = new Dictionary<string, Type>();
            foreach (var assembly in assemblies)
            {
                var pluginTypes = assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                foreach (var plugin in pluginTypes)
                {
                    var attr = (LoadPlugin)plugin.GetCustomAttribute(typeof(LoadPlugin));
                    if (attr != null)
                    {
                        types.Add(attr.Name, plugin);
                    }
                }
            }

            var loadedPlugins = new Stack<string>();
            foreach (var plugin in types)
            {
                InitPlugin(types, loadedPlugins, plugin.Key);
            }
        }
    }
}
