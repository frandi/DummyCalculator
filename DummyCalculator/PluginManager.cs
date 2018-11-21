using DummyCalculator.Plugins.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DummyCalculator
{
    public class PluginManager
    {
        private Dictionary<OperationType, List<(string pluginName, string pluginDll)>> _plugins;

        private readonly List<string> _pluginLocations;

        public PluginManager(IEnumerable<string> pluginLocations)
        {
            _pluginLocations = pluginLocations.ToList();
        }

        public void RefreshPlugins()
        {
            _plugins = new Dictionary<OperationType, List<(string pluginName, string pluginDll)>>();

            foreach (var location in _pluginLocations)
            {
                var files = Directory.GetFiles(location, "*.dll", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var info = Assembly.LoadFile(file);
                    if (info.EntryPoint != null && info.EntryPoint.DeclaringType.BaseType == typeof(CalculatorOperation))
                    {
                        var type = info.EntryPoint.DeclaringType.FullName;
                        var instance = (CalculatorOperation) info.CreateInstance(type, false, BindingFlags.ExactBinding, null, new object[]{ }, null, null);
                        if (!_plugins.ContainsKey(instance.Type))
                            _plugins.Add(instance.Type, new List<(string, string)> {(instance.Name, file)});
                        else
                            _plugins[instance.Type].Add((instance.Name, file));
                    }
                }
            }
        }

        public void AddPluginLocation(string location)
        {
            _pluginLocations.Add(location);
        }

        public (string pluginName, string pluginDll) GetPlugin(OperationType type, string name = "")
        {
            if (_plugins != null && _plugins.ContainsKey(type))
            {
                return _plugins[type].FirstOrDefault(p => string.IsNullOrEmpty(name) || p.pluginName == name);
            }

            return ("", "");
        }
    }
}
