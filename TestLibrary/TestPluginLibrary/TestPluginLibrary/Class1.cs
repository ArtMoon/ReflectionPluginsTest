using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ds.test.impl;

namespace TestPluginLibrary
{
    public class MathPlugin : IPlugin
    {
        public string PluginName { get; } = "Math Plugin";

        public string Version { get; } = "1.0.0";

        public System.Drawing.Image Image => throw new NotImplementedException();

        public string Description { get; } = "Make sum";

        public int Run(int input1, int input2)
        {
            return input1 + input2;
        }
    }
}
