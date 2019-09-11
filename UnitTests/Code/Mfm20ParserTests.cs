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
        private Mfm20Parser _paser;

        [TestInitialize]
        public void SetUp()
        {
            _paser = new Mfm20Parser();
        }

        [TestMethod()]
        public void GetItems_MustReturn_CorrectItemsCount()
        {
            //arrange
            var strHtml = File.ReadAllText(Const.FileHtmlTop20Page);
            //act
            var items = _paser.GetItems(strHtml);

            //assert
            Assert.AreEqual(5, items.Count, "items.count is invalid");
        }

        [TestMethod()]
        public void GetItems_MustReturn_CorrentItems()
        {
            //arrange
            var strHtml = File.ReadAllText(Const.FileHtmlTop20Page);
            //act
            var items = _paser.GetItems(strHtml);

            //assert
            //Assert.AreEqual(5, items.Count, "items.count is invalid");

            Assert.AreEqual("http://mfm.ua/wp-content/uploads/2018/01/Inna-Nirvana-MFM.UA_.mp3", items[0], "items[0] is invalid");
            Assert.AreEqual("http://mfm.ua/wp-content/uploads/2018/01/Taylor-Swift-Look-What-U-Made-Me-Do-MFM.UA_.mp3", items[19], "items[19] is invalid");
            Assert.AreEqual("http://mfm.ua/wp-content/uploads/2018/01/Arash-Helena-Dooset-Daram-FIlatov-Karas-Mix.mp3", items[4], "items[4] is invalid");
        }
    }
}