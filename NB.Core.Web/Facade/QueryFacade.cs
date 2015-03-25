using NB.Core.Valuation;
using NB.Core.Web.DownloadClient;
using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Enums;
using NB.Core.Web.Interfaces;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
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
        

        public async Task<PotentialTarget> GetFairValue (string ticker)
        {
            var data = new PotentialTarget(ticker);
            var tenYearsBondYield = DownloadHelper.GetQuote(new string[] { "^TNX" }, QuoteProperty.LastTradePriceOnly, QuoteProperty.ChangeInPercent);
            var currentPrice = DownloadHelper.GetQuote(new string[] { ticker }, QuoteProperty.LastTradePriceOnly, QuoteProperty.ChangeInPercent);
            var cpi = DownloadHelper.GetCpiData();
            var earningForecast = GetNasdaqEarningForecast(ticker);

            await cpi;
            await tenYearsBondYield;
            await currentPrice;
            await earningForecast;
            data.CurrentMarketPrice = currentPrice.Result.Items[0].LastTradePriceOnly;
            var fairValue = FairValueEngine.DiscountedCurrentValue(
                earningForecast.Result.YearlyEarningForecasts[0].ConsensusEpsForecast,
                earningForecast.Result.Years, 
                earningForecast.Result.YearlyEarningGrowth, 
                cpi.Result, 
               (double) tenYearsBondYield.Result.Items [0][QuoteProperty.LastTradePriceOnly]);
            data.FairValue = fairValue;
            return data;
        }

        private async Task<NasdaqEarningForecastAggregate> GetNasdaqEarningForecast(string ticker)
        {
            var setting = new NasdaqEarningForecastSetting(ticker);
            var downloader = new NasdaqEarningForecastDownloader(setting);
            var result = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
            return result;
        }

        
    }
}
