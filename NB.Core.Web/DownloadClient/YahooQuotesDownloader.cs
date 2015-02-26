using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.DownloadClient
{
    public class YahooQuotesDownloader : BaseDownloader<YahooQuotesResult>
    {
        public YahooQuotesDownloader(BaseSetting setting)
            : base(setting)
        {
            
        }

        protected override YahooQuotesResult ConvertResult(string contentStr, string ticker = "")
        {
            YahooQuotesData[] items=null;
            YahooQuotesDownloadSettings set = (YahooQuotesDownloadSettings)this.Setting;
            items = FinanceHelper.ImportExport.ToQuotesData(contentStr,
                                                              ',',
                                                              set.Properties,
                                                              CultureInfo.GetCultureInfo("en-US"));
            
            return new YahooQuotesResult(items, set);
        }
    }
}
