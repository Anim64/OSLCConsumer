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

        public OSLCManager(string server, string jtsServer, string login, string password, string projectName, float version = 3.0f)
        {
            this.Server = server;
            this.JtsServer = jtsServer;
            this.Login = login;
            this.Password = password;
            this.ProjectName = projectName;
            this.Version = version;

            ServicePointManager
                .ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;

            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new Uri(server);

           
                




                /*using (var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })
                {

                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        };
                    using (var client = new HttpClient(handler))
                    {
                        /*ServicePointManager.ServerCertificateValidationCallback =
                        delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        {
                            return true;
                        };
                        client.BaseAddress = new Uri("https://158.196.141.113/rm/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf + xml"));
                        httpclient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

                        HttpResponseMessage response = client.GetAsync("rootservices").Result;
                        response.EnsureSuccessStatusCode();
                        string result = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine("Result: " + result);
                    }
                }*/
        }

        public string GetServiceProviderCatalog()
        {
            string rootServices = "rootservices";
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf+xml"));
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

            //HttpResponseMessage response = HttpUtils.GetWebDocument(rootServices ,this.JtsServer, this.Login, this.Password, this.HttpClient);
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
            
            
            XNamespace xpath = "http://open-services.net/xmlns/rm/1.0/";
            //XName
            string attribute = xDoc.Root.Element(xpath + "rmServiceProviders").FirstAttribute.Value;
            //var list = xDoc.Descendants(xpath + "rmServiceProviders");



            response.Dispose();

            return attribute;


        }

        public string getServiceProviders(string catalogURI){
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf+xml"));
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
            
            

            
                XPathDocument doc = new XPathDocument(response.Content.ReadAsStreamAsync().Result);
                XPathNavigator nav = doc.CreateNavigator();
                XmlNamespaceManager manager = new XmlNamespaceManager(nav.NameTable);
                manager.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                manager.AddNamespace("oslc_rm", "http://open-services.net/xmlns/rm/1.0/");
                XPathNodeIterator iterator = nav.Select("/rdf:Description/oslc_rm:rmServiceProviders/@rdf:resource", manager);
                nav = doc.CreateNavigator();
                manager = new XmlNamespaceManager(nav.NameTable);
                manager.AddNamespace("oslc", "http://open-services.net/ns/core#");
                manager.AddNamespace("dcterms", "http://purl.org/dc/terms/");
                iterator = nav.Select("//oslc:ServiceProvider/dcterms:title", manager);
                Console.WriteLine("The Project Areas for this RM Server are:");
                bool fetched = false;
                while (iterator.MoveNext())
                {
                    fetched = true;
                    XPathNavigator nav2 = iterator.Current.Clone();
                    Console.WriteLine(nav2.Value);
                }
                if(!fetched)
                    Console.WriteLine("--No Project Areas Found--");
            


            

            return "test";


        }

         public string getServiceProvider(string catalogURI,String paName){
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf+xml"));
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
            
            

            
                XPathDocument doc = new XPathDocument(response.Content.ReadAsStreamAsync().Result);
                XPathNavigator nav = doc.CreateNavigator();
                XmlNamespaceManager manager = new XmlNamespaceManager(nav.NameTable);
                manager.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                manager.AddNamespace("oslc_rm", "http://open-services.net/xmlns/rm/1.0/");
                XPathNodeIterator iterator = nav.Select("/rdf:Description/oslc_rm:rmServiceProviders/@rdf:resource", manager);
                nav = doc.CreateNavigator();
                manager = new XmlNamespaceManager(nav.NameTable);
                manager.AddNamespace("oslc", "http://open-services.net/ns/core#");
                manager.AddNamespace("dcterms", "http://purl.org/dc/terms/");
                iterator = nav.Select("//oslc:ServiceProvider/dcterms:title", manager);
                bool fetched = false;
                while (iterator.MoveNext())
                {
                    fetched = true;
                    XPathNavigator nav2 = iterator.Current.Clone();
                    if (nav2.Value == paName)
                        return nav2.Value;
                }
                if(!fetched){
                    Console.WriteLine("--No Project Areas Found--");
                }
            


            

            return "null";


        }

        




        
    }
}
