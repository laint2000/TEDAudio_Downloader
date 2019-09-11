using Mp3Downloader.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mp3Downloader.Code
{
    public class Downloader
    {
        private const string SiteURL = "https://podbay.fm/podcast/523121474";

        private readonly IHttpConnectror _httpConnectror;
        private readonly IHtmlParser _htmlParser;
        private readonly IMp3FilesAdapter _mp3FilesAdapter;

        public event Action<bool, string> OnNewSongsListGet = delegate { };
        public event Action<string, bool, string> OnDownloadMusicFile = delegate { };
        public event Action AfterDownloadAllComplete = delegate { };
        public List<WebItemDTO> NewSongsList { get; }
        public List<string> NewFilesList => NewSongsList.Select(i => i.Name).ToList();

        public List<string> ErrorMessages { get; }

        private const bool ResultSuccesfull = true;
        private const bool ResultFailed = false;


        public Downloader(IHttpConnectror httpConnectror, IHtmlParser htmlParser, IMp3FilesAdapter mptFilesAdapter)
        {
            NewSongsList = new List<WebItemDTO>();
            ErrorMessages = new List<string>();

            _httpConnectror = httpConnectror;
            _httpConnectror.OnLoadStringComplete += HttpConnectror_OnNewSongsListComplete;
            _httpConnectror.OnLoadStreamComplete += HttpConnectror_OnMp3DownloadComplete;
            _htmlParser = htmlParser;
            _mp3FilesAdapter = mptFilesAdapter;
        }

        private void HttpConnectror_OnMp3DownloadComplete(string url, Stream mp3Stream)
        {
            var fileName = url.UrlFileNameOnly();

            _mp3FilesAdapter.SaveToFile(fileName, mp3Stream);

            OnDownloadMusicFile(url, ResultSuccesfull, "");            
        }

        public void GetNewSongsList()
        {
            ErrorMessages.Clear();
            NewSongsList.Clear();
            try
            { 
                _httpConnectror.LoadString(SiteURL);
            }
            catch (Exception e)
            {
                ErrorMessages.Add(e.Message);
                OnNewSongsListGet(ResultFailed, e.Message);
            }
        }

        private void HttpConnectror_OnNewSongsListComplete(string htmlText)
        {
            try
            {
                var list = _htmlParser.GetItems(htmlText);

                var hash = new HashSet<string>(_mp3FilesAdapter.ExistedFiles);
                var newSongs = list.Where(q => !hash.Contains(q.Name)).ToList();

                NewSongsList.AddRange(newSongs);
            }
            catch (Exception e)
            {
                ErrorMessages.Add(e.Message);
            }

            OnNewSongsListGet(ResultSuccesfull, "");
        }

        public void DownloadAllMp3()
        {
            DownloadAllMp3SongsInThread();
        }

        private void DownloadAllMp3SongsInThread()
        {
            new Task(DownloadAllMp3Songs).Start();
        }

        private void DownloadAllMp3Songs()
        {
            foreach (var item in NewSongsList)
            {
                try
                {
                    _httpConnectror.LoadStream(item.Url);
                }
                catch (Exception e)
                {
                    ErrorMessages.Add(e.Message);
                    OnDownloadMusicFile(item.Url, ResultFailed, e.Message);
                }
            }

            AfterDownloadAllComplete();
        }
    }
}