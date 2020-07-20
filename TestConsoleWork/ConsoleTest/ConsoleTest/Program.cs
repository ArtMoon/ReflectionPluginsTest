using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ds.test.impl;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Plugins.Instance.BindAssembly("TestPluginLibrary.dll");
            Console.WriteLine(Plugins.Instance.GetPlugin("Math Plugin").Run(3,4).ToString());
            Console.ReadLine();

        }
        
    }
}
