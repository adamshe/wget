using NB.Core.Web.DownloadClient;
using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Enums;
using NB.Core.Web.Interfaces;
using NB.Core.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Facade
{
    public class QueryFacade : IValuationQuery
    {
        /// <summary>
        /// <example>
        ///   setting.Properties = new QuoteProperty[] {
        //    QuoteProperty.LastTradePriceOnly,
        //    QuoteProperty.ChangeInPercent,
        //    QuoteProperty.OneyrTargetPrice,
        //    QuoteProperty.PercentChangeFromFiftydayMovingAverage,
        //    QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage            
        //   };
        /// </example>
        /// </summary>
        /// <param name="tickers"></param>
        /// <param name="quoteProperties"></param>
        /// <returns></returns>
        public  async Task<YahooQuotesAggregate> GetQuote(string[] tickers, params QuoteProperty[] quoteProperties)
        {          
            var setting = new YahooQuotesSettings();
            setting.IDs = tickers;
            var downloader = new YahooQuotesDownloader(setting);
            var result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            return result;          
        }

        public async Task<PotentialTarget> GetFairValue (string ticker)
        {
            var data = new PotentialTarget();
            var tenYearsBondYield = await GetQuote(new string[] { "^TNX" }, QuoteProperty.LastTradePriceOnly, QuoteProperty.ChangeInPercent);
            var currentPrice = await GetQuote(new string[] { ticker }, QuoteProperty.LastTradePriceOnly, QuoteProperty.ChangeInPercent);

            data.CurrentMarketPrice = currentPrice.Items[0].LastTradePriceOnly;
            // var fairValue = FairValueEngine.DiscountedCurrentValue(eps, 3, growthRate / 100.0, inflation, fixincomeReturnRate);
            return data;
        }
    }
}
