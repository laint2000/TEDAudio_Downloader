using System;
using System.IO;
using Mp3Downloader.Code;
using ProgramTests.Code.Tests;

namespace ProgramTests.Code.Fakes
{

    internal class FakeHttpConnector : IHttpConnectror
    {
        public event Action<string> OnLoadStringComplete;
        public event Action<string, string, Stream> OnLoadStreamComplete;

        public Stream LoadStream(string name, string url)
        {
            const string content = "Mp3 test content";
            var result = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            OnLoadStreamComplete(name, url, result);

            return result;
        }

        public string LoadString(string url)
        {
            var responseString = File.ReadAllText(Const.FileHtml_SiteMainPage);

            OnLoadStringComplete(responseString);
            return responseString;
        }
    }
}
