using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace JazzOSLCReqManager.Utils
{
    static internal class HttpUtils
    {
        static public bool DEBUG = true;

        internal static string AUTHURL = "X-jazz-web-oauth-url";
        internal static string AUTHREQUIRED = "X-com-ibm-team-repository-web-auth-msg";
        // name of custom header that authentication messages are stored in
        private static string FORM_AUTH_HEADER = "X-com-ibm-team-repository-web-auth-msg"; //$NON-NLS-1$
        // auth header value when authentication is required
        private static string FORM_AUTH_REQUIRED_MSG = "authrequired"; //$NON-NLS-1$
        // auth header value when authentication failed
        private static string FORM_AUTH_FAILED_MSG = "authfailed"; //$NON-NLS-1$
        // URI the server redirects to when authentication fails
        public static string FORM_AUTH_FAILED_URI = "/auth/authfailed"; //$NON-NLS-1$


        internal static void printResponseHeaders(HttpResponseMessage response)
        {
            HttpResponseHeaders headers = response.Headers;

            foreach (var header in headers)
            {
                Console.WriteLine("\t- " + header.Key + ": " + header.Value);
            }

        }

        internal static void printResponseBody(HttpResponseMessage response)
        {
            HttpContent content = response.Content;

            if (content == null)
                return;

            Console.WriteLine(content.ReadAsStringAsync().Result);


        }


        private static bool DoRRCOAuth(HttpResponseMessage documentResponse,string login,string password,HttpClient httpClient){

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
            if (DEBUG)
                Console.WriteLine("Cannot Perform login Operation Status Code : " + documentResponse.StatusCode.ToString());

                return false;

        }


        internal static HttpResponseMessage sendGetForSecureDocument(string RequestUri, string login, string password, HttpClient httpClient, string JtsUri)
        {

            if (DEBUG) 
                Console.WriteLine(">> GET(1) " + RequestUri);
            HttpResponseMessage documentResponse = httpClient.GetAsync(RequestUri).Result;

            if (DEBUG)
            {
                Console.WriteLine(">> Response Headers:");
                HttpUtils.printResponseHeaders(documentResponse);
            }

            bool loginResult = DoRRCOAuth(documentResponse,login,password,httpClient);
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

        internal static HttpResponseMessage sendPostForSecureDocument(string requestURI,string login,string password,
           HttpClient httpClient,HttpContent ValuesToSend){


           
            
            if (DEBUG)
                Console.WriteLine(">> Post(1) " + requestURI);
            HttpResponseMessage response = httpClient.PostAsync(requestURI,ValuesToSend).Result;
            if(DEBUG){
                Console.WriteLine(">> Response Headers:");
			    HttpUtils.printResponseHeaders(response);
            }

            //bool loginResult = DoRRCOAuth(response, login, password, httpClient);
            //if (loginResult)
            //{
            //    if (DEBUG)
            //        Console.WriteLine(">> GET(2) " + requestURI);
            //    response = httpClient.PostAsync(requestURI, ValuesToSend).Result;
            //    Console.WriteLine(response.StatusCode.ToString());
            //    return response;
            //}
            //else
            //    if (DEBUG)
            //    Console.WriteLine("Somethign went wrong during Authentication");
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured during Post method status code:" + response.StatusCode.ToString());
                response.Dispose();
            }
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            return response;

            
            

        }

        internal static HttpResponseMessage sendPutForSecureDocument(string requestURI,string login,string password,
           HttpClient httpClient,HttpContent ValuesToSend){
            if(DEBUG)
                Console.WriteLine(">> Put(1) " + requestURI);
            HttpResponseMessage response = httpClient.PutAsync(requestURI,ValuesToSend).Result;

            if(DEBUG){
                Console.WriteLine(">> Response Headers:");
			    HttpUtils.printResponseHeaders(response);
            }

            bool loginResult = DoRRCOAuth(response,login,password,httpClient);

            if(loginResult){
                response = httpClient.PutAsync(requestURI,ValuesToSend).Result;
            }

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch(Exception e)
            { 
                Console.WriteLine("Error occured during Put method");
                Console.WriteLine(e.InnerException);
                response.Dispose();
            }
            return response;
            


            }

    }
}
