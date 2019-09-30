using System;
using System.IO;
using System.Net;

namespace Mp3Downloader.Code
{
    public class HttpConnector : IHttpConnectror
    {
        public event Action<string> OnLoadStringComplete = delegate { };
        public event Action<string, string, Stream> OnLoadStreamComplete = delegate { };

        public string LoadString(string url)
        {
            var request = CreateRequest(url);

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            OnLoadStringComplete(responseString);
            return responseString;
        }

        public Stream LoadStream(string name, string url)
        {
            var request = CreateRequest(url);
            var response = (HttpWebResponse)request.GetResponse();
            var resultStream = response.GetResponseStream();

            OnLoadStreamComplete(name, url, resultStream);
            return resultStream;
        }

        private HttpWebRequest CreateRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";

            return request;
        }
    }
}