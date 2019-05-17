using JazzOSLCReqManager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace JazzOSLCReqManager
{
    internal class OSLCManager
    {
        
        public static bool DEBUG = false;
        public string Server { get; }
        public string JtsServer { get; }
        public string Login { get; }
        public string Password { get; }
        public string ProjectName { get; set; }
        public float Version { get; }
        public HttpClient HttpClient { get; }
        private static XNamespace xpath = "http://open-services.net/xmlns/rm/1.0/";
        private Dictionary<string,XNamespace> namespaces;


        

        public OSLCManager(string server, string jtsServer, string login, string password, string projectName, float version = 3.0f)
        {
            this.Server = server;
            this.JtsServer = jtsServer;
            this.Login = login;
            this.Password = password;
            this.ProjectName = projectName;
            this.Version = version;
            this.namespaces = new Dictionary<string, XNamespace>();
            this.namespaces.Add("oslc_rm","http://open-services.net/xmlns/rm/1.0/");
            this.namespaces.Add("oslc","http://open-services.net/ns/core#");
            this.namespaces.Add("dcterms","http://purl.org/dc/terms/");
            this.namespaces.Add("rdf","http://www.w3.org/1999/02/22-rdf-syntax-ns#");

            ServicePointManager
                .ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;

            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new Uri(server);
        }

        public bool CreateRequirement(string serviceProviderUrl, string parentFolder)
        {
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

            HttpResponseMessage response = HttpUtils.sendGetForSecureDocument(serviceProviderUrl, this.Login, this.Password, this.HttpClient, this.JtsServer);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                response.Dispose();
            }

            string resourceType = "http://open-services.net/ns/rm#Requirement";

            //This is the URL we can use to post requirements to (note there could be more than one, we just pick the first
            string requirementFactoryUrl = "//oslc:CreationFactory/oslc:resourceType[@rdf:resource=\"" + resourceType + "\"]/../oslc:creation/@rdf:resource";

            //This path tells us all the different types we can use to create our requirement
            string requirementfactoryShapes = "//oslc:CreationFactory/oslc:resourceType[@rdf:resource=\"" + resourceType + "\"]/../oslc:resourceShape/@rdf:resource";

            XDocument xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            response.Dispose();
            Console.WriteLine(xDoc.Document);


            XElement factoryNode = xDoc.Descendants(this.namespaces["oslc"] + "resourceType")
                                    .Where(rt => rt.FirstAttribute.Value == resourceType)
                                    .Ancestors(this.namespaces["oslc"] + "CreationFactory").FirstOrDefault();

            string factoryURI = factoryNode.Element(this.namespaces["oslc"] + "creation")
                                    .FirstAttribute.Value;

            var resourceShapes = factoryNode.Elements(this.namespaces["oslc"] + "resourceShape");

            string shapeURL = resourceShapes.FirstOrDefault().FirstAttribute.Value;

            

            Console.WriteLine(factoryURI);


            return true;
        }

        private string createRequirementFromShape(string shapeURL, string parentFolder)
        {
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

            HttpResponseMessage response = HttpUtils.sendGetForSecureDocument(shapeURL, this.Login, this.Password, this.HttpClient, this.JtsServer);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                response.Dispose();
            }

            XDocument xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            response.Dispose();

            //For this example, lets assume we just want to have a title, description and 
            //some basic content to add to primary text.
            string title = "MyDocument";
            string description = "This is a test document";
            //Note: primary text must be in xhtml compliant format
            string primaryText = "<div xmlns=\"http://www.w3.org/1999/xhtml\" id=\"_Nf2cQJKNEd25PMUBGiN3Dw\"><h1 id=\"_DwpWsMueEd28xKN9fhQheA\">Test Document</h1></div>";

            return null;
        }

        public string GetServiceProviderCatalog()
        {
            string rootServices = "rootservices";
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

            HttpResponseMessage response = HttpUtils.sendGetForSecureDocument("rm/"+rootServices, this.Login, this.Password, this.HttpClient, this.JtsServer);
            
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException);
                response.Dispose();
            }

            XDocument xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            response.Dispose();

            //XName
            string attribute = xDoc.Root.Element(namespaces["oslc_rm"] + "rmServiceProviders").FirstAttribute.Value;



            

            return attribute;


        }

        public Dictionary<string,string> GetServiceProviders(string catalogURI){
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

            HttpResponseMessage response = HttpUtils.sendGetForSecureDocument(catalogURI,this.Login,this.Password,this.HttpClient,this.JtsServer);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException);
                response.Dispose();
            }
            
            
            XDocument xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            response.Dispose();


            Dictionary <string,string> projectAreas = xDoc.Descendants(namespaces["dcterms"] + "title").ToDictionary(kv => kv.Value, kv => kv.Parent.FirstAttribute.Value);
            
            if(projectAreas == null)
            { 
                 Console.WriteLine("No project areas found");
                 return null;
            }
                   
            return projectAreas;
        }

        public string GetServiceProvider(string catalogURI){
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

            HttpResponseMessage response = HttpUtils.sendGetForSecureDocument(catalogURI,this.Login,this.Password,this.HttpClient,this.JtsServer);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException);
                response.Dispose();
            }
            
            XDocument xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            response.Dispose();

            string serviceProvider = xDoc.Descendants(namespaces["dcterms"] + "title").FirstOrDefault(p => p.Value == this.ProjectName).Parent.FirstAttribute.Value;
   
            return serviceProvider;


        }

        




        
    }
}
