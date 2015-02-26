using NB.Core.Web.DownloadSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadClient
{
    public class YahooHistoryCsvDownloader : BaseDownloader<FileInfo>
    {
        public YahooHistoryCsvDownloader(BaseSetting setting)
            : base(setting)
        {
            
        }
        protected override FileInfo ConvertResult(string contentStr, string ticker="")
        {
            throw new InvalidOperationException("this downloader should use BatchDownloadFilesTaskAsync");
        }

      
    }
}
