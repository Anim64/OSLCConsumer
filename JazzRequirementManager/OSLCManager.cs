using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace JazzRequirementManager
{
    public class OSLCManager
    {
        public class OSLCManager
        {
            static public bool DEBUG = false;
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

                

                using (var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })
                {
                    handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                    handler.ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        };
                    
    
                    this.HttpClient = new HttpClient(handler);
                    this.HttpClient.BaseAddress = new Uri(server);
                }




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

            

              
	}


}
    }
}
