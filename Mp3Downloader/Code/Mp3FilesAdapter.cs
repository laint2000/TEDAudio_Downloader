using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mp3Downloader.Code
{
    public class Mp3FilesAdapter : IMp3FilesAdapter
    {
        private readonly IFilesWriterReader _filesWriterReader;
        
        private const string Mp3Folder = "mp3";
        private const string MusicListName = "DownloadedFiles.txt";
        private int _fileCounter;

        public const int MaxDownloadedItemsLog = 1000;
        public List<string> ExistedFiles { get; private set; }
        public Mp3FilesAdapter(IFilesWriterReader filesWriterReader)
        {
            _filesWriterReader = filesWriterReader;
            var fileList = _filesWriterReader.GetStringList(MusicListName);

            ExistedFiles = new List<string>();
            ExistedFiles.AddRange(fileList);

            _fileCounter = GetMaxFileCounter();
        }

        private int GetMaxFileCounter()
        {
            List<string> files = _filesWriterReader.GetMp3FilesList(Mp3Folder);
            if (files.Count == 0) return 0;
            
            files.Sort();
            return ExtractFileCounter(files.Last());
        }

        private int ExtractFileCounter(string fileName)
        {
            var matches = Regex.Matches(fileName, @"^\d{1,5}");

            if (matches.Count < 1) return 0;
            if (matches[0].Groups.Count < 1) return 0;

            var counter = matches[0].Groups[0].Value.Trim();
            int.TryParse(counter, out var result);

            return result;
        }

        public bool SaveToFile(string fileName, Stream stream)
        {
            _fileCounter++;
            var fileNameToSave = $"{_fileCounter:D4} {fileName}";
            _filesWriterReader.SaveToFile(Mp3Folder, fileNameToSave, stream);
            AddToDownloadedFileList(fileName);
            return true;
        }

        private void AddToDownloadedFileList(string fileName)
        {
            ExistedFiles.Add(fileName);
            while (ExistedFiles.Count > MaxDownloadedItemsLog)
            {
                ExistedFiles.RemoveAt(0);
            }
            _filesWriterReader.SaveList(MusicListName, ExistedFiles);
        }
    }
}
