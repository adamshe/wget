using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NB.Core.Web.DownloadClient;
using NB.Core.Web.DownloadSettings;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using NB.Core.Web.Enums;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;

namespace NB.Core.Web.UnitTest
{
    [TestClass]
    public class DownloaderTest
    {
        string tickers = @"TQQQ,BIB,CURE,AAPL,YHOO,MSFT,GOOGL,CSCO,BRCM,INTC,CYBR,BA,ADBE,HDP,NEWR,WYNN,LVS,TSLA,NFLX,PCLN,AMZN,
            FB,LNKD,TWTR,JD,JMEI,TKMR,CELG,REGN,BIIB,ICPT,PCYC,INCY,DATA,NOW,GILD,SPLK,TSO,
            LNG,EOG,APC,GPRO,NUAN,RCL,MCO,DFS,AXP,MA,V,GS,BAC,JPM,
            C,JUNO,KITE,BLUE,GMCR";

        string index = @"SPY,IWM,TQQQ,BIB,CURE,XLE,XLF,EEM,FXI,RTH, XTN";//TZA,TNA,
        [TestMethod]
        public  async Task YahooHistoryCsvDownloaderTest()
        {
            var task = GetScheduledTasks();
            Debug.Write(task);
            var setting = new YahooHistoryCsvSetting("AAPL");
            var downloader = new YahooHistoryCsvDownloader(setting);
            var url = setting.GetUrl();
            Debug.WriteLine(url);
            var file = await downloader.DownloadFileTaskAsync().ConfigureAwait(false);
            var files = await downloader.BatchDownloadFilesTaskAsync(setting.GetUrls("AAPL,YAHOO,MSFT,GOOGL")).ConfigureAwait(false);

            Debug.WriteLine(file.ToString());
            foreach (var filestr in files)
                Debug.WriteLine(filestr.ToString());
          
        }

        [TestMethod]
        public async Task FinvizEarningCalendarDownloaderTest()
        {         
            var setting = new FinvizEarningCalendarSetting("CYBR");
            var downloader = new FinvizEarningCalendarDownloader(setting);
            var url = setting.GetUrl();
            Debug.WriteLine(url);
            var earningDate = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            var earningDates = await downloader.BatchDownloadObjectsTaskAsync(
                setting.GetUrls(tickers)).ConfigureAwait(false);

            Debug.WriteLine(earningDate.ToString());
            foreach (var earning in earningDates)
                Debug.WriteLine(earning.Ticker.PadRight(5) + " " + earning.ToString());

            var filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\earningCalendar.txt";
            using (StreamWriter file = File.CreateText(filePath))
            {
                foreach (var earning in earningDates)
                    file.WriteLine(earning.Ticker.PadRight(5) + " " + earning.ToString());
            }
        }

        [TestMethod]

        public async Task YahooQuotesDownloaderTest()
        {
            var setting = new YahooQuotesDownloadSettings();
            setting.Properties = new QuoteProperty[] {
                QuoteProperty.LastTradePriceOnly,
                QuoteProperty.ChangeInPercent,
                QuoteProperty.OneyrTargetPrice,
                QuoteProperty.PercentChangeFromFiftydayMovingAverage,
                QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage            
            };

            setting.IDs = MyHelper.GetStringToken(tickers, new string[] { ";", "," });
            var downloader = new YahooQuotesDownloader(setting);
            var results = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            

            results.SortBy(QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage);
            foreach (var result in results.Items)
            {
                Debug.WriteLine(string.Format("{0}     {1}               {2}           {3}          {4}          {5}         {6}", 
                    result.ID.PadRight(5),
                    result.Name.PadLeft(17),
                    result.LastTradePriceOnly.ToString().PadLeft(9),
                    result.ChangeInPercent.ToString().PadLeft(9),
                    result[QuoteProperty.OneyrTargetPrice].ToString().PadLeft(8),
                    result[QuoteProperty.PercentChangeFromFiftydayMovingAverage].ToString().PadLeft(8),
                    result[QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage].ToString().PadLeft(8)
                    ));
            }
        }
        protected IEnumerable<Task> GetScheduledTasks()
        {
            yield break;
        }
    }
}
