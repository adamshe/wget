using NB.Core.Web.DownloadClient;
using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Enums;
using NB.Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Utility
{
    public static class DownloadHelper
    {
        public static async Task<double> GetCpiData()
        {
            var setting = new CpiDataSetting();
            var downloader = new CpiDataDownloader(setting);
            var result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            var cpi = result.CurentCpi;

            return cpi;
        }

        public static async Task<double> GetRiskFreeRate()
        {
            var setting = new YahooQuotesSetting();
            setting.IDs = new string [] {"^TNX"};
            var downloader = new YahooQuotesDownloader(setting);
            var result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            return result.Items[0].LastTradePriceOnly;            
        }

        public static async Task<YahooQuotesAggregate> GetQuote(string[] tickers, params QuoteProperty[] quoteProperties)
        {
            var setting = new YahooQuotesSetting();
            setting.IDs = tickers;
            setting.Properties = quoteProperties;
            var downloader = new YahooQuotesDownloader(setting);
            var result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            return result;
        }
    }
}
