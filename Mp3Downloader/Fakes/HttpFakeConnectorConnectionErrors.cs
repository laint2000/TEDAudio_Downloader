using System;
using System.IO;
using Mp3Downloader.Code;

namespace Mp3Downloader.Fakes
{
    internal class HttpFakeConnectorConnectionErrors : IHttpConnectror
    {
        public event Action<string> OnLoadStringComplete = delegate { };
        public event Action<string, string, Stream> OnLoadStreamComplete = delegate { };

        public Stream LoadStream(string name, string url)
        {
            throw new Exception($"Connection error");
        }

        public string LoadString(string url)
        {
            throw new Exception($"Connection error");
        }
    }
}