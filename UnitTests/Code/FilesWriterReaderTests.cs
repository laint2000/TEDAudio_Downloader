using System;
using System.Collections.Generic;
using Mp3Downloader.Code;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProgramTests.Code.Tests
{
    [TestClass()]
    public class FilesWriterReaderTests
    {
        private FilesWriterReader _fileReaderWriter;

        [TestInitialize]
        public void TestInitialize()
        {
            _fileReaderWriter = new FilesWriterReader();
        }

        [TestMethod()]
        public void GetStringListTest()
        {
            //act
            var items = _fileReaderWriter.GetStringList(Const.FileMp3MusicList);

            //assert
            Assert.AreEqual(20, items.Count, "items.count is invalid");

            Assert.AreEqual("MFM-Matthew-Koma-Kisses-Back.mp3", items[0], "items[0] is invalid");
            Assert.AreEqual("MFM-Bebe-Rexha-I-Got-You.mp3", items[19], "items[19] is invalid");
        }

        [TestMethod]
        public void OpenUnexistFile_ReturnEmptyList()
        {
            var items = new List<string>();
            //act
            try
            {
                items = _fileReaderWriter.GetStringList("non_exists_file.txt");
            }
            catch (Exception e)
            {
                Assert.Fail($"Exception was trown: {e.Message}");
            }

            //assert
            Assert.AreEqual(0, items.Count, "items.count is invalid");
        }
    }
}