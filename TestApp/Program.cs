using JazzOSLCReqManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            OSLCManager man = new OSLCManager("https://158.196.141.113/rm/", "", "", "", "");
            Console.WriteLine(man.GetServiceProviderCatalog());
        }
    }
}
