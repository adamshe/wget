using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using System;
using System.Globalization;
using System.IO;

namespace NB.Core.Web.DownloadClient
{
    public class YahooQuotesDownloader : BaseDownloader<YahooQuotesResult>
    {
        public YahooQuotesDownloader(BaseSetting setting)
            : base(setting)
        {
            
        }

        protected override YahooQuotesResult ConvertResult(StreamReader sr, string ticker = "")
        {
            throw new NotImplementedException("stream is only for csv files.");
        }

        protected override YahooQuotesResult ConvertResult(string contentStr, string ticker = "")
        {
            YahooQuotesData[] items=null;
            YahooQuotesDownloadSettings set = (YahooQuotesDownloadSettings)this.Setting;
            items = FinanceHelper.ImportExport.ToQuotesData(contentStr,
                                                              ',',
                                                              set.Properties,
                                                              MyHelper.DefaultCulture);
            
            return new YahooQuotesResult(items, set);
        }
    }
}
