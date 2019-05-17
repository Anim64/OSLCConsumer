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
    public class OSLCManager
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
           
            
            //XName
            string attribute = xDoc.Root.Element(namespaces["oslc_rm"] + "rmServiceProviders").FirstAttribute.Value;



            response.Dispose();

            return attribute;


        }

        public Dictionary<string,string> getServiceProviders(string catalogURI){
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
            
                
                var nodes = xDoc.Descendants(namespaces["dcterms"]+"title");
                Dictionary <string,string> projectAreas = new Dictionary<string, string>();
                if(nodes != null)
                    foreach (var node in nodes){
                    projectAreas.Add(node.Value,node.Parent.FirstAttribute.Value);
                    }
                else{
                    Console.WriteLine("No project areas found");
                    return null;
                }
                    




            return projectAreas;


        }

         public string getServiceProvider(string catalogURI,String paName){
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
                
                string attribute = xDoc.Descendants(namespaces["dcterms"]+"title").FirstOrDefault(p => p.Value == paName).Parent.FirstAttribute.Value;

             

   
            return attribute;


        }

        




        
    }
}
