using System;
using System.Collections.Generic;
using System.Linq;
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
            
            foreach(var header in headers)
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
    }
}
