using Mp3Downloader.DTO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mp3Downloader.Code
{
    public interface IHttpConnectror
    {
        event Action<string> OnLoadStringComplete;
        event Action<string, Stream> OnLoadStreamComplete;
        string LoadString(string url);
        Stream LoadStream(string url);
    }

    public interface IHtmlParser
    {
        List<WebItemDTO> GetItems(string htmlText);
    }

    public interface IMp3FilesAdapter
    {
        List<string> ExistedFiles { get; }
        bool SaveToFile(string fileName, Stream stream);
    }

    public interface IFilesWriterReader
    {
        List<string> GetStringList(string fileName);
        void SaveToFile(string path, string fileName, Stream stream);
        void SaveList(string fileName, List<string> list);
        List<string> GetMp3FilesList(string mp3Folder);
    }
}