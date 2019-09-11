using Mp3Downloader.DTO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mp3Downloader.Code
{
    public class Mfm20Parser : IHtmlParser
    {
        private const string HttpSite = "http://mfm.ua/";
        private const string RegExpPattern = @"<a class=""track sp-play-track"" href=""(.+?\.mp3)""";

        public List<WebItemDTO> GetItems(string htmlText)
        {



            return new List<WebItemDTO>() {
                new WebItemDTO("FileName1", "http:\\filename1.mp3"),
                new WebItemDTO("FileName2", "http:\\filename2.mp3"),
                new WebItemDTO("FileName3", "http:\\filename3.mp3"),
                new WebItemDTO("FileName4", "http:\\filename4.mp3"),
                new WebItemDTO("FileName5", "http:\\filename5.mp3")
            };
        }

        public IEnumerable<HtmlNode> ParsePage(string htmlPage)
        {

            // Load the document using HTMLAgilityPack as normal
            var html = new HtmlDocument();
            html.LoadHtml(htmlPage);
            var document = html.DocumentNode;

            return document.QuerySelectorAll(".row.list-bordered");
        }

        private string GetFullUrlName(string urlName)
        {
            if (urlName.ToLower().StartsWith("http"))
            {
                return urlName;
            }

            var urlSuffix = urlName.StartsWith("/")
                ? urlName.Remove(0, 1)
                : urlName;

            return HttpSite + urlSuffix;
        }
    }
}