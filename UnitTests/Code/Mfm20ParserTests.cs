using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mp3Downloader.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProgramTests.Code.Tests
{
    [TestClass()]
    public class Mfm20ParserTests
    {
        private HtmlParser _paser;

        [TestInitialize]
        public void SetUp()
        {
            _paser = new HtmlParser();
        }

        [TestMethod()]
        public void GetSectionList_MustReturn_ValidValue()
        {
            //arrange
            var strHtml = File.ReadAllText(Const.FileHtml_SiteMainPage);

            var expectedFileName = $"{Const.FolderExpected}/section.html";
            var failedFileName = $"{Const.FolderFailed}/section_failed.html";
            File.Delete(failedFileName);
            
            //act
            var items = _paser.GetSectionsList(strHtml);

            //assert
            Assert.IsTrue(items.Any(), "No Sections were returned");

            var firstSection = items.First().InnerHtml;
            var expectedSection = File.ReadAllText(expectedFileName);
            if (expectedSection != firstSection) {
                File.WriteAllText(failedFileName, firstSection);
                Assert.Fail($"Sectiion data is not same as in {expectedFileName}. \n Please see result in {failedFileName} file");
            }
        }

        [TestMethod()]
        public void ParseSection_MustReturn_ValidValue()
        {
            //arrange
            var strHtml = File.ReadAllText(Const.FileHtml_SiteMainPage);
            var items = _paser.GetSectionsList(strHtml);
            Assert.IsTrue(items.Any(), "No Sections were returned");
            var firstSection = items.First();

            //act
            var result = _paser.ParseSection(firstSection);

            //assert
            Assert.AreEqual("Peering Deeper Into Space", result.Name, "Name is invalid");
            var expectedUrl = "https://play.podtrac.com/npr-510298/edge1.pod.npr.org/anon.npr-podcasts/podcast/npr/ted/2019/09/20190905_ted_peeringintospace-a88b2c7d-d37b-4caf-a6d0-a8b87a546e94.mp3?awCollectionId=510298&amp;awEpisodeId=758025242&amp;orgId=1&amp;d=3171&amp;p=510298&amp;story=758025242&amp;t=podcast&amp;e=758025242&amp;size=50616468&amp;ft=pod&amp;f=510298";
            Assert.AreEqual(expectedUrl, result.Url, "Url is invalid");
        }


        [TestMethod()]
        public void GetItems_MustReturn_CorrectItemsCount()
        {
            //arrange
            var strHtml = File.ReadAllText(Const.FileHtml_SiteMainPage);

            //act
            var items = _paser.GetItems(strHtml).ToList();

            //assert
            Assert.AreEqual(100, items.Count, "items.count is invalid");
        }

        [TestMethod()]
        public void GetItems_MustReturn_CorrentItems()
        {
            //arrange
            var strHtml = File.ReadAllText(Const.FileHtml_SiteMainPage);
            //act
            var items = _paser.GetItems(strHtml).ToList();

            //assert
            //Assert.AreEqual(5, items.Count, "items.count is invalid");

            Assert.AreEqual("Peering Deeper Into Space", items[0].Name, "items[0] is invalid");
            Assert.AreEqual("Keeping Secrets", items[19].Name, "items[19] is invalid");
            var expectedUrl = "https://play.podtrac.com/npr-510298/edge1.pod.npr.org/anon.npr-podcasts/podcast/npr/ted/2019/04/20190425_ted_secretspodcast-f3bbba67-ffce-49ac-8f06-afff46089014-427da72a-4510-4e7b-9548-6e212d4ad36b.mp3?awCollectionId=510298&amp;awEpisodeId=717119988&amp;orgId=1&amp;d=3133&amp;p=510298&amp;story=717119988&amp;t=podcast&amp;e=717119988&amp;size=50012352&amp;ft=pod&amp;f=510298";
            Assert.AreEqual(expectedUrl, items[19].Url, "items[19] is invalid");
        }
    }
}