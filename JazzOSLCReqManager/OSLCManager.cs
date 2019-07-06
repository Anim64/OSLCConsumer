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
using System.Web;
using System.IO;

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
            this.Namespaces.Add("rdfs", "http://www.w3.org/2000/01/rdf-schema#");
            this.Namespaces.Add("nav", "http://jazz.net/ns/rm/navigation#");
            this.Namespaces.Add("dc", "http://purl.org/dc/terms/");
            this.Namespaces.Add("oslc_rm2", "http://open-services.net/ns/rm#");
            this.Namespaces.Add("j.0", "http://www.ibm.com/xmlns/rdm/types/");
            this.Namespaces.Add("jazz_rm", "http://jazz.net/ns/rm#");

            ServicePointManager
                .ServerCertificateValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;

            this.HttpClient = new HttpClient();
            this.HttpClient.BaseAddress = new Uri(server);
        }

        public string getQueryCapability(string serviceProviderUri)
        {
            this.HttpClient.DefaultRequestHeaders.Clear();
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf+xml"));
            this.HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

            HttpResponseMessage response = HttpUtils.sendGetForSecureDocument(serviceProviderUri, this.Login, this.Password, this.HttpClient, this.JtsServer);

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

            string requestQueryURL = xDoc.Root.Descendants(this.Namespaces["dcterms"] + "title")
               .Where(p => p.Value == "Query Capability").FirstOrDefault()
               .Parent.Descendants(this.Namespaces["oslc"] + "queryBase")
               .FirstOrDefault().FirstAttribute
               .Value;

 
            return requestQueryURL;
        }

        public string getFolderQuery(string serviceProviderUri)
        {
            this.HttpClient.DefaultRequestHeaders.Clear();
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf+xml"));
            this.HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

            HttpResponseMessage response = HttpUtils.sendGetForSecureDocument(serviceProviderUri, this.Login, this.Password, this.HttpClient, this.JtsServer);

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

            string requestQueryURL = xDoc.Root.Descendants(this.Namespaces["dcterms"] + "title")
               .Where(p => p.Value == "Folder Query Capability").FirstOrDefault()
               .Parent.Descendants(this.Namespaces["oslc"] + "queryBase")
               .FirstOrDefault().FirstAttribute
               .Value;


            return requestQueryURL;
        }

        public void getRequirmentsFromCollection(HttpResponseMessage response)
        {
            this.HttpClient.DefaultRequestHeaders.Clear();
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            this.HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");
            XDocument xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);

            string coll = xDoc.Root.Descendants(this.Namespaces["oslc_rm2"] + "RequirementCollection").FirstOrDefault()
                .Attribute(this.Namespaces["rdf"]+"about").Value;

            HttpResponseMessage something = HttpUtils.sendGetForSecureDocument(coll, this.Login, this.Password, this.HttpClient, this.JtsServer);

            //Console.WriteLine(something.Content.ReadAsStringAsync().Result);

            HttpResponseMessage testing = HttpUtils.sendGetForSecureDocument("https://158.196.141.113/rm/delivery-sessions", this.Login, this.Password, this.HttpClient, this.JtsServer);

            Console.WriteLine(testing.Content.ReadAsStringAsync().Result);

        }


        public HttpResponseMessage performQuery(string queryCapabilityURI,string id)
        {
            this.HttpClient.DefaultRequestHeaders.Clear();
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf+xml"));
            this.HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");
            
            Encoding UTF8 = System.Text.Encoding.GetEncoding("UTF-8");
            
           string oslcSearchByIdentifierQuery = queryCapabilityURI
                + "&oslc.prefix=" + "dcterms=<http://purl.org/dc/terms/>"
                + "&oslc.select=" + "dcterms:title"
                + "&oslc.where=" + "dcterms:identifier=" + id;

            string[] lhrs = oslcSearchByIdentifierQuery.Split('?');
            string url = lhrs[0];
            string body = lhrs[1];


            StringContent stringContent = new StringContent(body,UTF8,"application/x-www-form-urlencoded");

            HttpResponseMessage response = HttpUtils.sendPostForSecureDocument(url, this.Login, this.Password, this.HttpClient, stringContent);

            /*XDocument xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            string coll = xDoc.Root.Descendants(this.Namespaces["oslc_rm2"] + "RequirementCollection").FirstOrDefault()
                .Attribute(this.Namespaces["rdf"]+"about").Value;

            HttpResponseMessage something = HttpUtils.sendGetForSecureDocument(coll, this.Login, this.Password, this.HttpClient, this.JtsServer);

            Console.WriteLine(something.Content.ReadAsStringAsync().Result);*/



            return response;

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
        //change to 'private' later
        public string DiscoverRootFolder(string ServiceProvider){

            this.HttpClient.DefaultRequestHeaders.Clear();
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            this.HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

            HttpResponseMessage response = HttpUtils.sendGetForSecureDocument(ServiceProvider,this.Login,this.Password,this.HttpClient,this.JtsServer);
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
            //XDocument xDoc = XDocument.Load("C:/Users/User/source/repos/OSLCConsumer-master/TestApp/bin/Debug/service.xml");
            Console.WriteLine(xDoc.Document.ToString());
            string QueryCapabilityURI = xDoc.Root.Descendants(this.Namespaces["dcterms"]+"title")
                .Where(p => p.Value == "Folder Query Capability").FirstOrDefault()
                .Parent.Descendants(this.Namespaces["oslc"]+"queryBase")
                .FirstOrDefault().FirstAttribute
                .Value;

            response.Dispose();

         

            response = HttpUtils.sendGetForSecureDocument(QueryCapabilityURI,this.Login,this.Password,this.HttpClient,this.JtsServer);

            try 
	        {	        
		        response.EnsureSuccessStatusCode();
	        }
	        catch (Exception e)
	        {

		        Console.WriteLine(e.InnerException);
                response.Dispose();
	        }


            xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            response.Dispose();
           
            string RootFolder = xDoc.Descendants(this.Namespaces["dcterms"]+"title")
                .Where(p => p.Value == "root").FirstOrDefault()
                .Parent.FirstAttribute
                .Value;


            return RootFolder;

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
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                response.Dispose();
            }

            if (response.Headers.Contains("X-com-ibm-team-repository-web-auth-msg"))
            {
                Console.WriteLine("Sign in was not successful");
                return null;
            }
            XDocument xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            response.Dispose();

            string serviceProvider = xDoc.Descendants(Namespaces["dcterms"] + "title").FirstOrDefault(p => p.Value == this.ProjectName).Parent.FirstAttribute.Value;
            return serviceProvider;


        }

        /// <summary>
        /// For testing purposes only
        /// </summary>
        public void TestRequirementRequest(string service)
        {
            /*RequirementRequest req = new RequirementRequest(this.Server, "", "TestShapeUrl");
            req.WriteXML(this.Namespaces);*/
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf+xml"));
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

            string queryCapabilityURI = this.getQueryCapability(service);

            Encoding UTF8 = System.Text.Encoding.GetEncoding("UTF-8");

            string oslcSearchByIdentifierQuery = queryCapabilityURI
                 + "&oslc.prefix=" + "dcterms=<http://purl.org/dc/terms/>"+ ",nav=<http://jazz.net/ns/rm/navigation#>" + ",oslc=<http://open-services.net/ns/core#>"
                 + "&oslc.select=" + "dcterms:title"
                 + "&oslc.where=" + "nav:parent=<https://158.196.141.113/rm/folders/_V1-4kTBiEemz5KZyNE5MOQ>";

            //string oslcSearchByIdentifierQuery = queryCapabilityURI;

            string[] lhrs = oslcSearchByIdentifierQuery.Split('?');
            string url = lhrs[0];
            string body = lhrs[1];


            StringContent stringContent = new StringContent(body, UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = HttpUtils.sendPostForSecureDocument(url, this.Login, this.Password, this.HttpClient, stringContent);
            string parrent = "http://jazz.net/ns/rm/navigation#";
            string folder = "https://158.196.141.113/rm/folders/_V1-4kTBiEemz5KZyNE5MOQ";
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            /*string req = "https://158.196.141.113/rm/resources/_dfmc8VRfEemJB4_OAAlcTQ"+ "?oslc.prefix=dcterms=http://purl.org/dc/terms/"+",nav="+parrent+"&oslc.properties=*"+"&oslc.where="+"nav:parent="+folder;
            response = HttpUtils.sendGetForSecureDocument(req, this.Login, this.Password, this.HttpClient, this.JtsServer);
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);*/

        }

        public Dictionary<string,string> getFoldersContainingArtifacts(string serviceProvider)
        {
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf+xml"));
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");

            string queryCapabilityURI = this.getFolderQuery(serviceProvider);

            HttpResponseMessage response = HttpUtils.sendGetForSecureDocument(queryCapabilityURI, this.Login, this.Password, this.HttpClient, this.JtsServer);

            XDocument xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            response.Dispose();

            string subfolders = xDoc.Descendants(Namespaces["nav"] + "subfolders").FirstOrDefault().Attributes().FirstOrDefault().Value;

            response = HttpUtils.sendGetForSecureDocument(subfolders, this.Login, this.Password, this.HttpClient, this.JtsServer);
            xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            response.Dispose();

            List<XElement> folderMembers = xDoc.Descendants(Namespaces["rdfs"] + "member").ToList();
            Dictionary<string, string> ArtifactFolders = new Dictionary<string, string>();
            foreach (var item in folderMembers)
            {
                string folderlink = item.Element(this.Namespaces["nav"] + "folder").Descendants(this.Namespaces["nav"] + "subfolders").FirstOrDefault().FirstAttribute.Value;

                response = HttpUtils.sendGetForSecureDocument(folderlink, this.Login, this.Password, this.HttpClient, this.JtsServer);
                xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                string subfolderName = "";
                string subfolderlink = "";
                try
                {
                    subfolderName = xDoc.Descendants(this.Namespaces["rdfs"] + "member").FirstOrDefault()
                    .Descendants(this.Namespaces["dcterms"] + "title").FirstOrDefault().Value;

                    subfolderlink = xDoc.Descendants(this.Namespaces["rdfs"] + "member").FirstOrDefault()
                    .Descendants(this.Namespaces["nav"] + "folder").FirstOrDefault().FirstAttribute.Value;
                }
                catch (Exception)
                {

                    Console.WriteLine("Title not found");
                }
                
                if (subfolderlink.Length > 1)
                    ArtifactFolders.Add(subfolderName, subfolderlink);


            }

            return ArtifactFolders;

        }

        public List<XElement> getRequirementsByFolder(string service, string folderURI)
        {
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf+xml"));
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");
            //Console.WriteLine(folderURI);
            string queryCapabilityURI = this.getQueryCapability(service);

            Encoding UTF8 = System.Text.Encoding.GetEncoding("UTF-8");

            string oslcSearchByIdentifierQuery = queryCapabilityURI
                 + "&oslc.prefix=" + "dcterms=<http://purl.org/dc/terms/>" + ",nav=<http://jazz.net/ns/rm/navigation#>" + ",oslc=<http://open-services.net/ns/core#>"
                 + "&oslc.select=" + "dcterms:title"
                 + "&oslc.where=" + "nav:parent=<"+folderURI+">";

            //string oslcSearchByIdentifierQuery = queryCapabilityURI;

            string[] lhrs = oslcSearchByIdentifierQuery.Split('?');
            string url = lhrs[0];
            string body = lhrs[1];


            StringContent stringContent = new StringContent(body, UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = HttpUtils.sendPostForSecureDocument(url, this.Login, this.Password, this.HttpClient, stringContent);
            XDocument xdoc =  XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            List<XElement> Requirements = xdoc.Descendants(this.Namespaces["oslc_rm2"] + "Requirement").ToList();

            return Requirements;


        }

        public List<Dictionary<string,string>> GetBodyOfRequirements(string service, List<XElement> reqs)
        {
            //XElement req = reqs[5];
            List<Dictionary<string, string>> requirementsProperties = new List<Dictionary<string, string>>();
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf+xml"));
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");


            /*foreach (var req in reqs)
            {
                Console.WriteLine(req.Descendants(this.Namespaces["dcterms"] + "title").FirstOrDefault().Value);
            }*/




            //var watch = System.Diagnostics.Stopwatch.StartNew();

            //foreach (var req in reqs)
            //{
            //    Dictionary<string, string> requirementProperties = new Dictionary<string, string>();


            //    string reqURI = req.FirstAttribute.Value;


            //    HttpResponseMessage response = HttpUtils.sendGetForSecureDocument(reqURI, this.Login, this.Password, this.HttpClient, this.JtsServer);

            //    XDocument xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
            //    string title = xDoc.Descendants(this.Namespaces["dcterms"] + "creator").FirstOrDefault().Parent.Descendants(this.Namespaces["dcterms"] + "title").FirstOrDefault().Value;
            //    string created = xDoc.Descendants(this.Namespaces["dcterms"] + "created").FirstOrDefault().Value;
            //    string creator = xDoc.Descendants(this.Namespaces["dcterms"] + "creator").FirstOrDefault().FirstAttribute.Value.Split('/').Last();
            //    string id = xDoc.Descendants(this.Namespaces["dcterms"] + "identifier").FirstOrDefault().Value;
            //    string description = xDoc.Descendants(this.Namespaces["jazz_rm"] + "primaryText").FirstOrDefault().Value;

            //    DateTime date = Convert.ToDateTime(created);
            //    Console.WriteLine(xDoc.Document.ToString());
            //    requirementProperties.Add("title", title);
            //    requirementProperties.Add("created", date.ToString());
            //    requirementProperties.Add("creator", creator);
            //    requirementProperties.Add("id", id);
            //    requirementProperties.Add("description", description);

            //    requirementsProperties.Add(requirementProperties);

            //}


            //////////////////////////////////////////////////////////////////

            Parallel.ForEach(reqs, (req) =>
             {
                 Dictionary<string, string> requirementProperties = new Dictionary<string, string>();


                 string reqURI = req.FirstAttribute.Value;


                 HttpResponseMessage response = HttpUtils.sendGetForSecureDocument(reqURI, this.Login, this.Password, this.HttpClient, this.JtsServer);
                 try { 
                 XDocument xDoc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                 string title = xDoc.Descendants(this.Namespaces["dcterms"] + "creator").FirstOrDefault().Parent.Descendants(this.Namespaces["dcterms"] + "title").FirstOrDefault().Value;
                 string created = xDoc.Descendants(this.Namespaces["dcterms"] + "created").FirstOrDefault().Value;
                 string modified = xDoc.Descendants(this.Namespaces["dcterms"] + "modified").FirstOrDefault().Value;
                 string creator = xDoc.Descendants(this.Namespaces["dcterms"] + "creator").FirstOrDefault().FirstAttribute.Value.Split('/').Last();
                 string id = xDoc.Descendants(this.Namespaces["dcterms"] + "identifier").FirstOrDefault().Value;
                 string description = xDoc.Descendants(this.Namespaces["jazz_rm"] + "primaryText").FirstOrDefault().Value;
                 string folder = xDoc.Descendants(this.Namespaces["nav"] + "parent").FirstOrDefault().FirstAttribute.Value;

                 DateTime date = Convert.ToDateTime(created);
                 DateTime modified_date = Convert.ToDateTime(modified);

                 //Console.WriteLine(xDoc.Document.ToString());
                 requirementProperties.Add("title", title);
                 requirementProperties.Add("created", date.ToString());
                 requirementProperties.Add("modified", modified_date.ToString());
                 requirementProperties.Add("creator", creator);
                 requirementProperties.Add("id", id);
                 requirementProperties.Add("description", description);
                 requirementProperties.Add("reqURI", reqURI);
                 requirementProperties.Add("folder", folder);

                 requirementsProperties.Add(requirementProperties);
                 }catch(Exception e)
                 {
                     requirementProperties.Add("title", null);
                     requirementProperties.Add("created", null);
                     requirementProperties.Add("modified", null);
                     requirementProperties.Add("creator", null);
                     requirementProperties.Add("id", null);
                     requirementProperties.Add("description", null);
                     requirementProperties.Add("reqURI", null);
                     requirementProperties.Add("folder", null);
                 }
             });
            /***************************************************/

            return requirementsProperties;


        }

        public XDocument CreateSnapshot(string service, List<XElement> reqs,string folder,string FileName)
        {
            List<Dictionary<string, string>> requirementsProperties = new List<Dictionary<string, string>>();
            this.HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/rdf+xml"));
            HttpClient.DefaultRequestHeaders.Add("OSLC-Core-Version", "2.0");
            XDocument xDocReqs = null;
            if (File.Exists(FileName))
            {
               xDocReqs = XDocument.Load(FileName);
            }
            else { 
                xDocReqs = new XDocument();
                xDocReqs.Add(new XElement("Snapshots"));
            }
            string date = DateTime.Now.ToString();
            Console.WriteLine(xDocReqs.Root);
            XElement Snapshot = new XElement("Snapshot",new XAttribute("Date",date),new XAttribute("Folder",folder));
            Parallel.ForEach(reqs, (req) =>
            {
                Dictionary<string, string> requirementProperties = new Dictionary<string, string>();


                string reqURI = req.FirstAttribute.Value;


                HttpResponseMessage response = HttpUtils.sendGetForSecureDocument(reqURI, this.Login, this.Password, this.HttpClient, this.JtsServer);
                try
                {
                    XElement Requirement = XElement.Parse(response.Content.ReadAsStringAsync().Result);
                    Snapshot.Add(Requirement);
                    //Console.WriteLine(xDoc.Document.ToString());
                    //xDocReqs.Element("Snapshot").Add(x);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Fail");
                }
            });
            xDocReqs.Root.Add(Snapshot);
            xDocReqs.Save(FileName);
            return xDocReqs;
        }

        public void LoadSnapshot(string filename,string folder = "all")
        {

            XDocument Snapshots_XML = XDocument.Load(filename);
            List<XElement> Snapshots = Snapshots_XML.Descendants("Snapshot").Where(x => x.Attribute("Folder").Value.Equals(folder)).ToList();


            if(Snapshots.Count > 1)
            {
                int i = 0;
                while(i < Snapshots.Count)
                {
                    if (i == Snapshots.Count - 1)
                        continue;
                    for(int c = 0;c < Snapshots[i].Descendants(this.Namespaces["rdf"] + "RDF").ToList().Count; c++)
                    {
                        XElement req1 = Snapshots[i].Descendants(this.Namespaces["rdf"] + "RDF").ToList()[c];
                        string id = req1.Descendants(this.Namespaces["dcterms"] + "identifier").FirstOrDefault().Value;
                        XElement req2 = Snapshots[i + 1].Descendants(this.Namespaces["rdf"] + "RDF")
                            .Where(x => x.Descendants(this.Namespaces["dcterms"] + "identifier").FirstOrDefault().Value == id).FirstOrDefault();
                        if (req1.Descendants(this.Namespaces["dcterms"]+"modified").FirstOrDefault().Value == req2.Descendants(this.Namespaces["dcterms"] + "modified").FirstOrDefault().Value)
                        {
                            Console.WriteLine("ok");
                        }
                        else
                        {
                            Console.WriteLine(req1.ToString());
                            Console.WriteLine(req2.ToString());
                        }
                    }






                    i++;

                }
            }


            Console.WriteLine("here");
        }







    }
}
