using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace JazzOSLCReqManager.Utils
{
    static internal class HttpUtils
    {
        static public bool DEBUG = true;

        public static string AUTHURL = "X-jazz-web-oauth-url";
        public static string AUTHREQUIRED = "X-com-ibm-team-repository-web-auth-msg";
        // name of custom header that authentication messages are stored in
        private static string FORM_AUTH_HEADER = "X-com-ibm-team-repository-web-auth-msg"; //$NON-NLS-1$
        // auth header value when authentication is required
        private static string FORM_AUTH_REQUIRED_MSG = "authrequired"; //$NON-NLS-1$
        // auth header value when authentication failed
        private static string FORM_AUTH_FAILED_MSG = "authfailed"; //$NON-NLS-1$
        // URI the server redirects to when authentication fails
        public static string FORM_AUTH_FAILED_URI = "/auth/authfailed"; //$NON-NLS-1$


        public static void printResponseHeaders(HttpResponseMessage response)
        {
            HttpResponseHeaders headers = response.Headers;

            foreach (var header in headers)
            {
                Console.WriteLine("\t- " + header.Key + ": " + header.Value);
            }

        }

        public static void printResponseBody(HttpResponseMessage response)
        {
            HttpContent content = response.Content;

            if (content == null)
                return;

            Console.WriteLine(content.ReadAsStringAsync().Result);


        }

        public static HttpResponseMessage GetWebDocument(string requestUri, string jtsUri, string login, string password, HttpClient client)
        {
            return null;
        }

        private static Boolean doRRCOAuth(HttpResponseMessage documentResponse,string login,string password,HttpClient httpClient){

            if (documentResponse.StatusCode == HttpStatusCode.OK)
            {
                var header = documentResponse.Headers.TryGetValues(AUTHREQUIRED, out var values) ? values.FirstOrDefault() : null;

                
                //string header = documentResponse.Headers[AUTHREQUIRED];
                if ((header != null) && header.Equals(FORM_AUTH_REQUIRED_MSG))
                {
                    documentResponse.Dispose();

                    var formVariables = new List<KeyValuePair<string, string>>();
                    formVariables.Add(new KeyValuePair<string, string>("j_username", login));
                    formVariables.Add(new KeyValuePair<string, string>("j_password", password));
                    var formContent = new FormUrlEncodedContent(formVariables);

                    HttpResponseMessage formPost = httpClient.PostAsync("/jts/auth/j_security_check", formContent).Result;

                    if (DEBUG) 
                        Console.WriteLine(">> POST " + formPost.RequestMessage.RequestUri);
                    HttpRequestMessage formResponse = formPost.RequestMessage;
                    if (DEBUG) 
                        HttpUtils.printResponseHeaders(formPost);

                    header = formResponse.Headers.TryGetValues(AUTHREQUIRED, out values) ? values.FirstOrDefault() : null;
                    if ((header != null) && header.Equals(FORM_AUTH_FAILED_MSG))
                    {
                        // The login failed
                        throw new WebException("Authentication failed");
                    }
                    else
                    {
                        formResponse.Dispose();
                        // The login succeed
                        return true;
                    }
                }
                else
                    return true;
            }

            return false;

        }


        public static HttpResponseMessage sendGetForSecureDocument(string RequestUri, string login, string password, HttpClient httpClient, string JtsUri)
        {

            if (DEBUG) 
                Console.WriteLine(">> GET(1) " + RequestUri);
            HttpResponseMessage documentResponse = httpClient.GetAsync(RequestUri).Result;

            if (DEBUG)
            {
                Console.WriteLine(">> Response Headers:");
                HttpUtils.printResponseHeaders(documentResponse);
            }

            Boolean loginResult = doRRCOAuth(documentResponse,login,password,httpClient);
            if(loginResult){
                if (DEBUG) 
                    Console.WriteLine(">> GET(2) " + RequestUri);
                return httpClient.GetAsync(RequestUri).Result;
               }
            else
                if (DEBUG) 
                    Console.WriteLine("Somethign went wrong during Authentication");
            
            return documentResponse;
        }

       public static HttpResponseMessage sendPostForSecureDocument(string requestURI,string login,string password,
           HttpClient httpClient,HttpContent ValuesToSend,int expectedResponse){
           if(DEBUG)
                Console.WriteLine(">> Post(1) " + requestURI);
           HttpResponseMessage response = httpClient.PostAsync(requestURI,ValuesToSend).Result;

            if(DEBUG){
                Console.WriteLine(">> Response Headers:");
			    HttpUtils.printResponseHeaders(response);
            }


            if((int)response.StatusCode != expectedResponse){
                Console.WriteLine("Error occured during Post method");
                response.Dispose();
            }
            return response;
            

        }

    }
}
