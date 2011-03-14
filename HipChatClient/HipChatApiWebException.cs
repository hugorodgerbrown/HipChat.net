using System;
using System.Net;

namespace HipChat
{
    /// <summary>
    /// Custom exception containing information returned from HipChat (extracted from WebException).
    /// </summary>
    /// <remarks>
    /// The HipChat API will return an HTTP 4XX in the case of an exception, and will put the error into the body of the response. 
    /// This exception extracts the relevant information and presents in a more logical format.
    /// More details here - https://www.hipchat.com/docs/api/response_codes
    /// </remarks>
    public class HipChatApiWebException: ApplicationException
    {
        private HttpStatusCode status;
 
        /// <summary>
        /// The HTTP status code returned by HipChat - usually a 4xx
        /// </summary>
        public HttpStatusCode Status { get { return status; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="closeResponse">If True, the WebException.Response object is closed.</param>
        public HipChatApiWebException(WebException ex, bool closeResponse): base(HttpUtils.ReadResponseBody(ex.Response))
        {
            this.status = ((HttpWebResponse)ex.Response).StatusCode;
            if (closeResponse)
            {
                ex.Response.Close();
            }
        }
    }
}
