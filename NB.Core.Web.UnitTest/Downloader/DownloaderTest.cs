using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NB.Core.Web.DownloadClient;
using NB.Core.Web.DownloadSettings;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using NB.Core.Web.Enums;
using NB.Core.Web.Utility;
using System.Linq;
using NB.Core.Web.Extensions;
namespace NB.Core.Web.UnitTest
{
    [TestClass]
    public class DownloaderTest
    {//^TNX,TQQQ";/*
        string tickers = @"BIB,CURE,AAPL,YHOO,MSFT,GOOGL,CSCO,BRCM,INTC,CYBR,BA,ADBE,HDP,NEWR,WYNN,LVS,TSLA,NFLX,PCLN,AMZN,
            FB,LNKD,TWTR,JD,JMEI,TKMR,CELG,REGN,BIIB,ICPT,PCYC,INCY,DATA,NOW,GILD,SPLK,TSO,
            LNG,EOG,APC,GPRO,NUAN,RCL,MCO,DFS,AXP,MA,V,GS,BAC,JPM,
            C,JUNO,KITE,BLUE,GMCR";

        string index = @"SPY,IWM,TQQQ,BIB,CURE,XLE,XLF,EEM,FXI,RTH, XTN";//TZA,TNA,

        [TestMethod]
        public void DateTimeTest()
        {//https://msdn.microsoft.com/en-us/library/zdtaw1bw%28v=vs.110%29.aspx
            Debug.WriteLine(DateTime.Now.ThisMonthNthOf(3, DayOfWeek.Friday));

            foreach (var date in DateTime.Now.ThisYearNthOf(3, DayOfWeek.Friday))
            {
                Debug.WriteLine(date.ToString("D"));
            }
            
        }

        [TestMethod]
        public async Task YahooHistoryCsvStreamDownloaderTest()
        {
            var task = GetScheduledTasks();
            Debug.Write(task);
            var setting = new YahooHistoryCsvSetting("QQQ");
            setting.Start = new DateTime(2014,5,1);
            setting.End = new DateTime(2014, 12, 31);
            var downloader = new YahooHistoryCsvDownloader(setting);
            var url = setting.GetUrl();
            Debug.WriteLine(url);
            var data = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
            var datas = await downloader.BatchDownloadObjectsStreamTaskAsync(setting.GetUrls("AAPL,YHOO,MSFT,GOOGL")).ConfigureAwait(false);
//BatchDownloadObjectsStreamTaskAsync //BatchDownloadObjectsTaskAsync
            Debug.WriteLine(data.ToString());
            foreach (var data1 in datas)
                Debug.WriteLine(data1.ToString());

        }

        [TestMethod]
        public  async Task YahooHistoryCsvDownloaderTest()
        {
            var task = GetScheduledTasks();
            Debug.Write(task);
            var setting = new YahooHistoryCsvSetting("QQQ");
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
            var result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            

            result.SortBy(QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage);
            foreach (var item in result.Items)
            {
                Debug.WriteLine(string.Format("{0}     {1}               {2}           {3}          {4}          {5}         {6}", 
                    item.ID.PadRight(5),
                    item.Name.PadLeft(17),
                    item.LastTradePriceOnly.ToString().PadLeft(9),
                    item.ChangeInPercent.ToString().PadLeft(9),
                    (item[QuoteProperty.OneyrTargetPrice]??0).ToString().PadLeft(8),
                    (item[QuoteProperty.PercentChangeFromFiftydayMovingAverage]??0).ToString().PadLeft(8),
                    (item[QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage]??0).ToString().PadLeft(8)
                    ));
            }
        }

        [TestMethod]
        public async Task TrefisDownloaderTest ()
        {
            var setting = new TrefisSetting();
            var downloader = new TrefisDownloader(setting);
            var result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            foreach (var item in result.Items)
            {
                Debug.WriteLine(string.Format("{0}     {1}        {2}       {3}       {4}      {5}              {6}",
                   item.Ticker.PadRight(5),
                   item.CompanyName.PadRight(29),
                   item.TrefisTarget.ToString("c").PadLeft(9),
                   item.MarketPrice.ToString("c").PadLeft(9),
                   item.PriceGap.ToString("P").PadLeft(9),
                   item.Sector.ToString().PadRight(18),
                   item.Industry.ToString().PadRight(18)
                //   item.Bearishness.ToString().PadLeft(8)
                   ));
            }

        }

        [TestMethod]
        public async Task FinvizDetailsBatchDownloaderTest()
        {
            var setting = new FinvizDetailsSetting("CSCO");
            var downloader = new FinvizDetailsDownloader(setting);
           var result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            // var results = await downloader.BatchDownloadObjectsTaskAsync(setting.GetUrls(tickers)).ConfigureAwait(false);
          //  foreach (var result in results)
            {
                var stringPropertyNamesAndValues = result.GetType()
                    .GetProperties()
                    .Select(pi => new
                    {
                        Name = pi.Name,
                        Value = pi.GetGetMethod().Invoke(result, null)
                    });

                foreach (var pair in stringPropertyNamesAndValues)
                {
                    Debug.WriteLine("Name: {0}                   -      Value: {1}", pair.Name.PadRight(36), pair.Value.ToString().PadLeft(6));
                }
            }
        }

        [TestMethod]
        public async Task FinvizDetailsDownloaderTest()
        {
            var setting = new FinvizDetailsSetting("AAPL");
            var downloader = new FinvizDetailsDownloader(setting);
            var result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            var stringPropertyNamesAndValues = result.GetType()
                .GetProperties()
                .Select(pi => new
                {
                    Name = pi.Name,
                    Value = pi.GetGetMethod().Invoke(result, null)
                });

            foreach (var pair in stringPropertyNamesAndValues)
            {
                Debug.WriteLine("Name: {0}             -      Value: {1}", pair.Name.PadRight(26), pair.Value.ToString().PadLeft(6));
            }
        }

      //  [TestMethod]
        public void SendSMS ()
        {
            long[] friendTmus = { 7184042681,7186668495  };
            long[] friendAtt = { 9172079948,8483912608 };
            var msg = @"亲爱的朋友, 您将自动成为Adam开发的交易系统的首批用户通知子系统
                 该系统现在主要研究是分析明牌，从重要网站数据采集，不犯方向性错误。主要关注四种策略，好骑师，灰马，BUYOUT和庄家自救，4种占齐胜算是95%以上，
                 系统主要是避免人性的弱点和心情的起伏，不会犯低级错误，
                 后面阶段将用NEURAL NETWORK建立预测系统，定价，趋势的提前预见，ORDER MANAGEMENT，机会寻找，胜算估计，自动交易。
                 BACKEND可能会用WEB API， 前台可能会是SPA";
            foreach (var phone in friendTmus)
            {
             //   MyHelper.SendTextMessage("慢点开车", "想", 7184042681, CarrierGateWay.TMOBILE);
             //   MyHelper.SendTextMessage("Test", "Hello, 你受到这封信，是因为你认识Adam，开发一个交易系统的通知子系统", 9172079948, CarrierGateWay.ATT);
                MyHelper.SendTextMessage("订阅服务 : ",
                msg, phone, CarrierGateWay.TMOBILE);
            }

            foreach (var phone in friendAtt)
            {
                //   MyHelper.SendTextMessage("慢点开车", "想", 7184042681, CarrierGateWay.TMOBILE);
                //   MyHelper.SendTextMessage("Test", "Hello, 你受到这封信，是因为你认识Adam，开发一个交易系统的通知子系统", 9172079948, CarrierGateWay.ATT);
                MyHelper.SendTextMessage("订阅服务 : ", msg
                    , phone, CarrierGateWay.ATT);
            }
        }

        protected IEnumerable<Task> GetScheduledTasks()
        {
            yield break;
        }
    }
}
