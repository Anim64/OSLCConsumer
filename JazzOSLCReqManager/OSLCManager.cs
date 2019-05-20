using JazzOSLCReqManager.Datamodel;
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
        private Dictionary<string,XNamespace> Namespaces;




        public OSLCManager(string server, string jtsServer, string login, string password, string projectName, float version = 3.0f)
        {
            this.Server = server;
            this.JtsServer = jtsServer;
            this.Login = login;
            this.Password = password;
            this.ProjectName = projectName;
            this.Version = version;
            this.Namespaces = new Dictionary<string, XNamespace>();
            this.Namespaces.Add("oslc_rm","http://open-services.net/xmlns/rm/1.0/");
            this.Namespaces.Add("oslc","http://open-services.net/ns/core#");
            this.Namespaces.Add("dcterms","http://purl.org/dc/terms/");
            this.Namespaces.Add("rdf","http://www.w3.org/1999/02/22-rdf-syntax-ns#");

            ServicePointManager
                .ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;

            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new Uri(server);
        }

        public string CreateRequirement(string serviceProviderUrl, string parentFolder)
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


            XElement factoryNode = xDoc.Descendants(this.Namespaces["oslc"] + "resourceType")
                                    .Where(rt => rt.FirstAttribute.Value == resourceType)
                                    .Ancestors(this.Namespaces["oslc"] + "CreationFactory").FirstOrDefault();

            string factoryURI = factoryNode.Element(this.Namespaces["oslc"] + "creation")
                                    .FirstAttribute.Value;

            var resourceShapes = factoryNode.Elements(this.Namespaces["oslc"] + "resourceShape");
            if(resourceShapes.Count() == 0)
            {
                Console.WriteLine("Could not find creation factories for project: " + this.ProjectName
                    + "\nPlease make sure there is at least one type in the type system");
            }

            string shapeURL = resourceShapes.FirstOrDefault().FirstAttribute.Value;

            Console.WriteLine(factoryURI);

            string content = CreateRequirementFromShape(shapeURL, parentFolder);

            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            HttpClient.DefaultRequestHeaders.Add("Content-Type", "application/xml");
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");
            StringContent httpContent = new StringContent(content);

            response = HttpUtils.sendPostForSecureDocument(factoryURI, this.Login, this.Password, this.HttpClient, httpContent);

            Uri location = response.Headers.Location;
            Console.WriteLine(location.OriginalString);

            response.Dispose();

            return location.OriginalString;
        }

        private string CreateRequirementFromShape(string shapeURL, string parentFolder)
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
            Console.WriteLine(xDoc.Document); 
            response.Dispose();

            //For this example, lets assume we just want to have a title, description and 
            //some basic content to add to primary text.
            string title = "MyDocument";
            string description = "This is a test document";
            //Note: primary text must be in xhtml compliant format
            string primaryText = "<div xmlns=\"http://www.w3.org/1999/xhtml\" id=\"_Nf2cQJKNEd25PMUBGiN3Dw\"><h1 id=\"_DwpWsMueEd28xKN9fhQheA\">Test Document</h1></div>";

            RequirementRequest req = new RequirementRequest(this.Server, "", shapeURL);
            req.DcTitle = title;
            req.DcDescription = description;

            if (parentFolder != null)
            {
                req.ParentFolder = parentFolder;
            }

            //Add any internal properties to request by getting property URI from shape
            string primaryTextPropURI = FindPropertyByTitle(xDoc, "Primary Text");
            req.RmLiteralProperties.Add(primaryTextPropURI, primaryText);

            return req.WriteXML().ToString();
        }

        public string FindPropertyByTitle(XDocument xmlDocument, string title)
        {
            string propertyDefinition = null;
            if (this.Version >= 4.0f)
            {
                propertyDefinition = xmlDocument.Descendants(this.Namespaces["dcterms"] + "title")
                                        .Where(t => t.Value == title)
                                        .Ancestors().FirstOrDefault()
                                        .Element(this.Namespaces["oslc"] + "propertyDefinition")
                                        .FirstAttribute
                                        .Value;
                                        
            }

            else
            {
                propertyDefinition = xmlDocument.Descendants(this.Namespaces["oslc"] + "name")
                                        .Where(t => t.Value == title.Replace(" ", ""))
                                        .Ancestors().FirstOrDefault()
                                        .Element(this.Namespaces["oslc"] + "propertyDefinition")
                                        .FirstAttribute
                                        .Value;
            }
            return propertyDefinition;
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
            string attribute = xDoc.Root.Element(Namespaces["oslc_rm"] + "rmServiceProviders").FirstAttribute.Value;

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


            Dictionary <string,string> projectAreas = xDoc.Descendants(Namespaces["dcterms"] + "title").ToDictionary(kv => kv.Value, kv => kv.Parent.FirstAttribute.Value);
            
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

            string serviceProvider = xDoc.Descendants(Namespaces["dcterms"] + "title").FirstOrDefault(p => p.Value == this.ProjectName).Parent.FirstAttribute.Value;
   
            return serviceProvider;


        }

        




        
    }
}
