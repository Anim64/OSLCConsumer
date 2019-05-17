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
            string projectArea = "Test project";
            OSLCManager man = new OSLCManager("https://158.196.141.113/", "", "fhanslik", "fhanslik", "");
            string providerCatalog = man.GetServiceProviderCatalog();
            Console.WriteLine(providerCatalog);
            string serviceProvider = man.getServiceProvider(providerCatalog,projectArea);
            Console.WriteLine(serviceProvider);
            Console.ReadKey();
            
        }
    }
}
