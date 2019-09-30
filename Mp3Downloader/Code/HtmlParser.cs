using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Mp3Downloader.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Mp3Downloader.Code
{
    public class HtmlParser : IHtmlParser
    {
        public IEnumerable<WebItemDTO> GetItems(string htmlText)
        {
            var sectionList = GetSectionsList(htmlText);
            var result = from section in sectionList
                         select ParseSection(section);

            return result.ToList();
        }

        public IEnumerable<HtmlNode> GetSectionsList(string htmlText)
        {
            var html = new HtmlDocument();
            html.LoadHtml(htmlText);

            var result = html.DocumentNode.QuerySelectorAll("div.preview.jsx-506443636");

            return result;
        }

        public WebItemDTO ParseSection(HtmlNode node)
        {
            var result = new WebItemDTO();

            var textInnerHtml = result.Name = node.QuerySelector("a.jsx-506443636.title-inner")?.InnerHtml;
            result.Name = textInnerHtml?
                .Replace("&#x27;", "")
                .Replace("?", "")
                .Replace(".", "")
                .Replace(":", "")
                .Replace("  ", " "); // <-- keep this last one

            result.Url = node.QuerySelector("div.jsx-506443636.download > a")?.Attributes["href"]?.Value;
            return result;
        }
    }
}