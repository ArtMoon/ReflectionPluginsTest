using System;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ds.test.impl
{
    public interface IPlugin
    { 
        string PluginName { get; }
        string Version { get; }
        System.Drawing.Image Image { get; }
        string Description { get; }
        int Run(int input1, int input2);

    }

    interface PluginFactory
    {
        int PluginCount { get; }
        string[] GetPluginNames { get; }
        IPlugin GetPlugin(string pluginName);
    }

    public class Plugins : PluginFactory
    {

        private Plugins() { }
        private Dictionary<string, IPlugin> _pluginsDisctionary = new Dictionary<string, IPlugin>();
        private static Plugins _instance = null;
        private Assembly _bindedAssembly = null;
        public static Plugins Instance
        {
            get 
            {   
                if(_instance == null)
                {
                    _instance = new Plugins();
                }

                return _instance;
            }
        }

        public int PluginCount
        {
            get
            {
                int i = 0;
                if(_bindedAssembly == null)
                {
                    throw new AssemblyException("No Binded Assembly!!!");
                }
                return _pluginsDisctionary.Count;
            }
        }

        public string[] GetPluginNames
        {
            get
            {
                if (_bindedAssembly == null)
                {
                    throw new AssemblyException("No Binded Assembly!!!");
                }

                return _pluginsDisctionary.Keys.ToArray(); 
            }
        }

        public IPlugin GetPlugin(string pluginName)
        {
            IPlugin plugin;
            _pluginsDisctionary.TryGetValue(pluginName, out plugin);

            if (plugin == null) throw new PluginException("No such plugin!!!");

            return plugin;
            
        }


        public void BindAssembly(string assemblyName)
        {
            _bindedAssembly = Assembly.LoadFrom(assemblyName);
            RegisterPlugins();
        }

        private void RegisterPlugins()
        {
            foreach (var t in _bindedAssembly.GetTypes())
            {
                if (t.GetInterface("IPlugin") != null)
                {
                    IPlugin plugin = (IPlugin) Activator.CreateInstance(t);
                    _pluginsDisctionary.Add(plugin.PluginName, plugin);
                }
            }
        }
    }

    class PluginException : Exception
    {
        public PluginException(string message)
            : base(message)
        { }
    }

    class AssemblyException : Exception
    {
        public AssemblyException(string message)
            : base(message)
        { }
    }

}
