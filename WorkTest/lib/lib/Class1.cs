using System;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ds.test.impl
{

    /// <summary>
    /// Plugin interface
    /// </summary>
    public interface IPlugin
    { 
        string PluginName { get; }
        string Version { get; }
        System.Drawing.Image Image { get; }
        string Description { get; }
        /// <summary>
        /// Main function of interface
        /// </summary>
        /// <param name="input1"> first param</param>
        /// <param name="input2"> second param</param>
        /// <returns></returns>
        int Run(int input1, int input2);

    }

    interface PluginFactory
    {
        int PluginCount { get; }
        string[] GetPluginNames { get; }
        IPlugin GetPlugin(string pluginName);
    }

    /// <summary>
    /// Class which can bind assembly and work with IPlugin implementations in it
    /// It's a singleton class
    /// </summary>
    public class Plugins : PluginFactory
    {

        private Plugins() { }
        private Dictionary<string, IPlugin> _pluginsDisctionary = new Dictionary<string, IPlugin>();
        private static Plugins _instance = null;
        /// <summary>
        /// Binded Assembly
        /// </summary>
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

        /// <summary>
        /// Bind assembly with plugins by assembly name
        /// </summary>
        /// <param name="assemblyName"> assembly name</param>
        public void BindAssembly(string assemblyName)
        {
            _bindedAssembly = Assembly.LoadFrom(assemblyName);
            if(_bindedAssembly != null) RegisterPlugins();
        }

        /// <summary>
        /// if assembly exists, we need to register all Plugins in it
        /// </summary>
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


    /// <summary>
    /// Some exceptions
    /// </summary>
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
