﻿using JazzOSLCReqManager;
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
            //RequirementHandler rqH = new RequirementHandler("https://158.196.141.113/", "", "fhanslik", "fhanslik", projectArea);
            //rqH.CreateRequirement();
            /*string providerCatalog = man.GetServiceProviderCatalog();
            Console.WriteLine(providerCatalog);
            //get single Project Area
            string serviceProvider = man.getServiceProvider(providerCatalog,projectArea);
            Console.WriteLine(serviceProvider);*/
            //Write all available project areas
            /*Dictionary<string,string> serviceProviders = man.getServiceProviders(providerCatalog);
            foreach(KeyValuePair<string,string> singleProvider in serviceProviders){
                Console.WriteLine("ProjectArea: "+singleProvider.Key + " ServiceProvider : "+singleProvider.Value);
            }*/

            OSLCManager man = new OSLCManager("https://158.196.141.113/","","fhanslik","fhanslik",projectArea);
            //string s = man.DiscoverRootFolder(projectArea);
            string catalogUri = man.GetServiceProviderCatalog();
            string service = man.GetServiceProvider(catalogUri);
            //man.DiscoverRootFolder(service);
            string query = man.getQueryCapability(service);
            man.performQuery(query);

            Console.ReadKey();

            
        }
    }
}
