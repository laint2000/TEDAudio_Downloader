using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mp3Downloader.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace ProgramTests.Code.Tests
{
    [TestClass()]
    public class Mp3FilesAdapterTests
    {
        private Mock<IFilesWriterReader> _filesWriterReader;
        private Mp3FilesAdapter _mp3FileAdapter;

        [TestInitialize]
        public void TestInitialize()
        {
            _filesWriterReader = new Mock<IFilesWriterReader>();
            _filesWriterReader.Setup(q => q.SaveList(It.IsAny<string>(), It.IsAny<List<string>>()));
            _filesWriterReader.Setup(q => q.GetStringList(It.IsAny<string>())).Returns(new List<string>());
            _filesWriterReader.Setup(q => q.SaveToFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()));
            _filesWriterReader.Setup(q => q.GetMp3FilesList(It.IsAny<string>())).Returns(new List<string>()
            {
                "0003 Shanguy-La-Louze.mp3",
                "0101 Camelphat-Elderbrook-Cola.mp3",
                "0007 Sofi-Tukker-Nervo-Best-Friend.mp3"
            });

            _mp3FileAdapter = new Mp3FilesAdapter(_filesWriterReader.Object);
        }

        [TestMethod()]
        public void SaveToFileTest()
        {
            var fileName = "SomeFileName.mp3";
            _mp3FileAdapter.SaveToFile(fileName, new MemoryStream());

            Assert.AreEqual(1, _mp3FileAdapter.ExistedFiles.Count, "Invalid Downloaded list count");
            Assert.AreEqual(fileName, _mp3FileAdapter.ExistedFiles[0], "Invalid file name");
        }

        [TestMethod()]
        public void SaveToFileTest_CheckMaxFilesLog()
        {
            //arrange
            for (var i = 0; i < Mp3FilesAdapter.MaxDownloadedItemsLog + 100; i++)
            {
                _mp3FileAdapter.ExistedFiles.Add($"FileName{i}");
            }
            var fileName = "SomeFileName.mp3";

            //act
            _mp3FileAdapter.SaveToFile(fileName, new MemoryStream());

            //assert
            Assert.AreEqual(Mp3FilesAdapter.MaxDownloadedItemsLog, _mp3FileAdapter.ExistedFiles.Count, "Invalid Downloaded list count");
            Assert.AreEqual(fileName, _mp3FileAdapter.ExistedFiles[Mp3FilesAdapter.MaxDownloadedItemsLog-1], "Invalid file name");
        }


        [TestMethod()]
        public void InitializeFileCounter()
        {
            //arrange
            var fileName = "SomeFileName";

            //act
            var stream = new MemoryStream();
            _mp3FileAdapter.SaveToFile(fileName, stream);

            //assert
            var expectedFileName = $"0102 {fileName}.mp3";
            _filesWriterReader.Verify(x => x.SaveToFile("mp3", expectedFileName, stream), Times.Once);

        }

    }
}