using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Downloader.Code
{
    public class FilesWriterReader : IFilesWriterReader
    {
        public List<string> GetStringList(string fileName)
        {
            if (!File.Exists(fileName))
                return new List<string>();

            return File.ReadAllLines(fileName).ToList();
        }

        public void SaveToFile(string path, string fileName, Stream stream)
        {
            Directory.CreateDirectory(path);
            var fullFileName = $"{path}\\{fileName}";

            if (File.Exists(fullFileName)) { File.Delete(fullFileName); }

            using (var fileStream = new FileStream(fullFileName, FileMode.CreateNew))
            {
                stream.CopyTo(fileStream);
                fileStream.Flush();
            }
        }

        public void SaveList(string fileName, List<string> list)
        {
            File.WriteAllLines(fileName, list);
        }

        public List<string> GetMp3FilesList(string mp3Folder)
        {
            var filesList =  Directory.GetFiles(mp3Folder, "*.mp3").ToList();
            
            return filesList.Select(r => Path.GetFileName(r)).ToList();
        }
    }
}
