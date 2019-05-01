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


        public static HttpResponseMessage sendGetForSecureDocument(string RequestUri, string login, string password, HttpClient httpClient, string JtsUri)
        {

            //HttpResponseMessage docGet2 = httpClient.GetAsync(RequestUri).Result;

            //if (request.Headers.Count() > 0)
            //{
            //    foreach (KeyValuePair<string, IEnumerable<string>> key in request.Headers)
            //    {
            //        try
            //        {
            //            docGet2.Headers.Add(key.Key, key.Value);
            //        }
            //        catch (ArgumentException)
            //        {

            //        }
            //    }
            //}
            if (DEBUG) Console.WriteLine(">> GET(1) " + RequestUri);
            HttpResponseMessage documentResponse = httpClient.GetAsync(RequestUri).Result;

            if (DEBUG)
            {
                Console.WriteLine(">> Response Headers:");
                HttpUtils.printResponseHeaders(documentResponse);
            }
            if (documentResponse.StatusCode == HttpStatusCode.OK)
            {
                var header = documentResponse.Headers.TryGetValues(AUTHREQUIRED, out var values) ? values.FirstOrDefault() : null;


                //string header = documentResponse.Headers[AUTHREQUIRED];
                if ((header != null) && header.Equals("authrequired"))
                {
                    documentResponse.Dispose();

                    var formVariables = new List<KeyValuePair<string, string>>();
                    formVariables.Add(new KeyValuePair<string, string>("j_username", login));
                    formVariables.Add(new KeyValuePair<string, string>("j_password", password));
                    var formContent = new FormUrlEncodedContent(formVariables);

                    //HttpResponseMessage formPost = httpClient.PostAsync("/j_security_check", formContent).Result;
                    HttpResponseMessage formPost = httpClient.PostAsync("/jts/auth/authrequired", formContent).Result;

                    if (DEBUG) Console.WriteLine(">> POST " + formPost.RequestMessage.RequestUri);
                    HttpRequestMessage formResponse = formPost.RequestMessage;
                    if (DEBUG) HttpUtils.printResponseHeaders(formPost);

                    header = formResponse.Headers.TryGetValues(AUTHREQUIRED, out values) ? values.FirstOrDefault() : null;
                    if ((header != null) && header.Equals("authfailed"))
                    {
                        // The login failed
                        throw new WebException("Authentication failed");
                    }
                    else
                    {
                        formResponse.Dispose();
                        // The login succeed
                        // Step (3): Request again the protected resource
                        if (DEBUG) Console.WriteLine(">> GET(2) " + RequestUri);
                        return httpClient.GetAsync(RequestUri).Result;
                    }
                }
            }
            return documentResponse;
        }

    }
}
