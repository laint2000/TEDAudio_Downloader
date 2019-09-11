using System;
using Mp3Downloader.Fakes;

namespace Mp3Downloader.Code
{
    static class Factory
    {
        public static Downloader CreateDownloader()
        {
            var httpConnector = new HttpConnector();
            var htmlParser = new Mfm20Parser();
            var filesWriterReader = new FilesWriterReader();
            var mp3FilesAdapter = new Mp3FilesAdapter(filesWriterReader);
            

            return new Downloader(httpConnector, htmlParser, mp3FilesAdapter);
        }

        public static Downloader CreateTestDownloader()
        {
            var httpConnector = new HttpTestConnector();
            var htmlParser = new Mfm20Parser();

            var filesWriterReader = new FilesWriterReader();
            var mp3FilesAdapter = new Mp3FilesAdapter(filesWriterReader);

            return new Downloader(httpConnector, htmlParser, mp3FilesAdapter);
        }

        public static Downloader CreateTestDownloaderWithErrors()
        {
            var httpConnector = new HttpFakeConnectorDownloadStreamError();
            //var httpConnector = new HttpFakeConnectorConnectionErrors();
            var htmlParser = new Mfm20Parser();

            var filesWriterReader = new FilesWriterReader();
            var mp3FilesAdapter = new Mp3FilesAdapter(filesWriterReader);

            return new Downloader(httpConnector, htmlParser, mp3FilesAdapter);
        }
    }
}
