using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3Downloader.Code
{
    public static class UrlUtils
    {
        public static string UrlFileNameOnly(this string urlString)
        {
            var lastDelimeter = urlString.LastIndexOf("/", StringComparison.Ordinal);
            return urlString.Substring(lastDelimeter + 1);
        }
    }
}
