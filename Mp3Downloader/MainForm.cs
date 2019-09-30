using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mp3Downloader.Code;

namespace Mp3Downloader
{
    public partial class MainForm : Form
    {

        private readonly Downloader _downloader;
        public MainForm()
        {
            InitializeComponent();

            _downloader = Factory.CreateDownloader();
            //_downloader = Factory.CreateTestDownloader();
            //_downloader = Factory.CreateTestDownloaderWithErrors();

            _downloader.OnNewSongsListGet += downloader_OnNewSongsListGet;
            _downloader.OnDownloadMusicFile += downloader_OnDownloadMusicFile; 
            _downloader.AfterDownloadAllComplete += downloader_AfterDownloadAllComplete;

            btnRefresh_Click(null, null);
        }


        private void downloader_OnNewSongsListGet(bool isSuccesfull, string errorMsg)
        {
            if (!isSuccesfull) {
                txtConsole.AppendText($"Error: {errorMsg} \r\n");
                return;
            }

            if (_downloader.NewFilesList.Count <= 0)
            {
                txtConsole.AppendText($"No new files \r\n");
                return;
            }

            var textToShow = string.Join("\r\n", _downloader.NewFilesList);
            txtConsole.AppendText($"{textToShow}\r\n");
        }

        private void downloader_OnDownloadMusicFile(string url, bool isSuccesfull, string errorMsg)
        {
            RunInMainStream(() => ShowDownloadMusicFileInformation(url, isSuccesfull, errorMsg));
        }

        private void downloader_AfterDownloadAllComplete()
        {
            RunInMainStream(() => ShowDownloadCompleteInformation());
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            txtConsole.Text = "";
            if (_downloader.NewSongsList.Count == 0)
            {
                txtConsole.AppendText($"No new music to download \r\n");
                return;
            }

            _downloader.DownloadAllMp3();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtConsole.Text = "Loading new songs list \r\n\r\n";
            _downloader.GetNewSongsList();
        }

        private void RunInMainStream(Action methodToRun)
        {
            // this is used to call method from other thread in main thread
            this.Invoke(new MethodInvoker(methodToRun));
        }

        private void ShowDownloadMusicFileInformation(string url, bool isSuccesfull, string errorMsg) {
            var fileName = url.UrlFileNameOnly();

            if (!isSuccesfull)
            {
                txtConsole.AppendText($"Error: {fileName} \r\n" +
                                      $"       {errorMsg} \r\n");
                return;
            }

            txtConsole.AppendText($"File saved: {fileName} \r\n");
        }

        private void ShowDownloadCompleteInformation()
        {
            txtConsole.AppendText("\r\n\r\n");
            txtConsole.AppendText("Download complete \r\n");
        }
    }
}
