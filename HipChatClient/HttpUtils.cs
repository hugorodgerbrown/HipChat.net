using System;
using System.IO;
using System.Net;
using System.Text;

namespace HipChat
{
    /// <summary>
    /// Http utilities class used for the API integration itself.
    /// </summary>
    class HttpUtils
    {
        /// <summary>
        /// Reads the entire contents of a web response stream and returns as a string.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static string ReadResponseBody(WebResponse response)
        {
            using (Stream receiveStream = response.GetResponseStream())
            {
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                using (StreamReader readStream = new StreamReader(receiveStream, encode))
                {
                    return readStream.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Calls the API, returns the contents of the response as a string
        /// </summary>
        /// <param name="request">An HTTP request object. This is not just a URL, as the request might be a POST, not a GET.</param>
        /// <returns>The raw response body - as JSON / XML</returns>
        /// <exception cref="HipChatApiWebException">Thrown if the API itself returns an error.</exception>
        internal static string CallApi(HttpWebRequest request)
        {
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // in the case of a HipChat exception, the reason is in the response body, so need to extract this.
                    using (Stream receiveStream = response.GetResponseStream())
                    {
                        Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                        using (StreamReader readStream = new StreamReader(receiveStream, encode))
                        {
                            return readStream.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException we)
            {
                throw new HipChatApiWebException(we, true);
            }
        }

        /// <summary>
        /// Converts a UNIX timestamp (seconds since midnight 1-Jan-1970) into a .net DateTime
        /// </summary>
        /// <param name="seconds">The number of seconds since midnight 1-Jan-1970</param>
        /// <returns></returns>
        internal static DateTime ConvertUnixTime(int seconds)
        {
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0);
            return time.AddSeconds(seconds);
        }

        /// <summary>
        /// Converts a UNIX timestamp (seconds since midnight 1-Jan-1970) into a .net DateTime
        /// </summary>
        /// <param name="seconds">The number of seconds since midnight 1-Jan-1970</param>
        /// <returns></returns>
        internal static DateTime ConvertUnixTime(string seconds)
        {
            int sec = int.Parse(seconds);
            return ConvertUnixTime(sec);
        }
    }
}
