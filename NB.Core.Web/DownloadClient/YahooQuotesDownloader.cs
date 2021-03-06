﻿using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using System;
using System.Globalization;
using System.IO;

namespace NB.Core.Web.DownloadClient
{
    public class YahooQuotesDownloader : BaseDownloader<YahooQuotesAggregate>
    {
        public YahooQuotesDownloader(YahooQuotesSetting setting)
            : base(setting)
        {
            
        }

        protected sealed override YahooQuotesAggregate ConvertResult(string contentStr, string ticker = "")
        {
            YahooQuotesData[] items=null;
            YahooQuotesSetting set = (YahooQuotesSetting)this.Setting;
            items = FinanceHelper.ImportExport.ToQuotesData(contentStr,
                                                              ',',
                                                              set.Properties,
                                                              MyHelper.DefaultCulture);
            
            return new YahooQuotesAggregate(items, set);
        }
    }
}
