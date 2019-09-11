using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Downloader.DTO
{
    public class WebItemDTO
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public WebItemDTO(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}
