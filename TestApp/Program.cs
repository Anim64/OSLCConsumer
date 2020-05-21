using JazzOSLCReqManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            
            string catalogUri = man.GetServiceProviderCatalog();
            string service = man.GetServiceProvider(catalogUri);
            man.GetServiceProviders(catalogUri);
            Dictionary<string,string> folders = man.getFoldersContainingArtifacts(service);
            List<XElement> reqs =  man.getRequirementsByFolder(service, folders["SYAR artifacts"]);
            List<Dictionary<string,string>> test =  man.GetBodyOfRequirements(service,reqs);
            XDocument x = man.CreateSnapshot(service, reqs,folders["SYAR artifacts"],"snapshots.xml");

            //man.LoadSnapshot("snapshots.xml", folders["SYAR artifacts"]);







            //man.TestRequirementRequest(service);
            //string s = man.DiscoverRootFolder(service);
            //man.DiscoverRootFolder(service);
            //string query = man.getQueryCapability(service);
            //HttpResponseMessage queryResponse =  man.performQuery(query,"1075");
            //man.getRequirmentsFromCollection(queryResponse);

            Console.ReadKey();

            
        }
    }
}
