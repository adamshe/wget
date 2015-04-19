using Microsoft.VisualStudio.TestTools.UnitTesting;
using NB.Core.Valuation;
using NB.Core.Web.DataAccess.Repository;
using NB.Core.Web.DownloadClient;
using NB.Core.Web.DownloadSettings;
using NB.Core.Web.Enums;
using NB.Core.Web.Extensions;
using NB.Core.Web.Models;
using NB.Core.Web.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;

namespace NB.Core.Web.UnitTest
{
    [TestClass]
    public class DownloaderTest
    {//^TNX,TQQQ";/*
        string[] friendTmus = { "7184042681", "7186668495" };
        string[] friendAtt = { "8483912608" }; //9172079948

        string[] friendsEmail = { "adamshe@gmail.com" };//"laofengs@gmail.com", 
        //"SPY,GMCR,AAPL,AMZN,PCLN,TRIP,EXPE,ISRG,CYBR,PANW,JD,GILD,JUNO,KITE,BLUE,MSFT,CSCO,NFLX,GOOGL";/
        string tickers = @"SPY,AAPL,YHOO,MSFT,GOOGL,CSCO,BRCM,INTC,CYBR,BA,ADBE,HDP,NEWR,WYNN,LVS,TSLA,NFLX,PCLN,AMZN,
            FB,LNKD,TWTR,JD,JMEI,DATA,NOW,GILD,SPLK,TSO,
            LNG,EOG,APC,GPRO,NUAN,RCL,MCO,DFS,AXP,MA,V,GS,BAC,JPM,
            C,JUNO,KITE,BLUE,GMCR,PCYC,INCY,GEVA,ACAD,TKMR,CELG,REGN,BIIB,ICPT";
                          

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
            setting.End = new DateTime(2014,12, 31);
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
        public async Task GoogleIntradyCsvDownloaderTest()
        {
            //1425907800
            DateTime now = MyHelper.DateTimeFromUnixTimestamp(1424874600, -300);
            Debug.WriteLine(now.ToString("MM-dd-yyyy hh:mm:ss"));

            var setting = new GoogleIntradayCsvSetting("CSCO");
            var downloader = new GoogleIntradayCsvDownloader(setting);
            var url = setting.GetUrl();
            Debug.WriteLine(url);
            var data = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
            foreach (var data1 in data)
            {
                PrintProperties(data1, 0);
            }

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
        public async Task YahooHistoryCvsDownloaderDataAnalysisTest()
        {
            var allTickers = tickers.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
            List<PriceStatisticsAggregate> list = new List<PriceStatisticsAggregate>();
            foreach (var ticker in allTickers)
            {
                try
                {
                    var setting = new YahooHistoryCsvSetting(ticker, -100);
                    var downloader = new YahooHistoryCsvDownloader(setting);
                    var data = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
                  
                    DateTime endDate = new DateTime(2015, 1, 29);//DateTime.Now.AddDays(-100);
                    DateTime startDate = endDate.AddDays(-72);//DateTime.Now.AddDays(-100);
                    var filterData = data.Where(point => point.Timestamp >= startDate && point.Timestamp <= endDate).ToList();
                    PriceStatisticsAggregate analysis = new PriceStatisticsAggregate(setting.Ticker, filterData);
                    analysis.SlideWindow = 5;
                    analysis.Partition();
                    analysis.RunPartitionAnalysis();

                    list.Add(analysis);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ticker + " " + ex.Message);
                }
            }
            var orderByVolumePartitions = list;/*.Where(a => //a.TotalNetChangeHandPercentage > 0 
                 (((a.MaxUpPercent.AverageDailyGainInDollar + a.MaxDownPercent.AverageDailyGainInDollar) > 0 ) &&
                 ((a.UpdayAverageGain + a.DowndayAverageGain) > 0) &&
                  a.TotalChangeHandPercentage >= 0.6)
                )
                .OrderByDescending(aggregate => aggregate.TotalChangeHandPercentage);*/

            foreach (var aggregate in orderByVolumePartitions)
            {
                try
                {
                    Console.WriteLine(aggregate.Ticker);
                    foreach (var partition in aggregate.Partitions)
                    {
                        Console.WriteLine("{0} {1} days  {2:MM/dd/yyyy} {3:MM/dd/yyyy} daily gain: {4:f2} stdev: {5:f2} - TOTAL gain {6:f2}  change hand {7:p}",
                            partition.Direction,
                            partition.Count,
                            partition.DataRange.First().Timestamp,
                            partition.DataRange.Last().Timestamp,
                            partition.AverageDailyGainInDollar,
                            partition.StDevStrength,
                            partition.DataRange.Last().Close - partition.DataRange.First().Open,
                            partition.ChangeHandPercentage
                            );
                    }

                    Console.WriteLine("{0} days, Expected {1:p} up gain {2:f2} Direction {3} average up {4:f2} - daily average up {5:f2}",
                        aggregate.MaxUpPercent.Count,
                        aggregate.MaxUpPercent.PriceRangePercent,
                        aggregate.MaxUpPercent.PriceRange,
                        aggregate.MaxUpPercent.Direction,
                        aggregate.MaxUpPercent.AverageDailyGainInDollar,
                        aggregate.UpdayAverageGain
                        );

                    Console.WriteLine("{0} days, Expected {1:p} down loss {2:f2}  Direction {3} average down {4:f2} -  daily average down {5:f3}",
                        aggregate.MaxDownPercent.Count,
                        aggregate.MaxDownPercent.PriceRangePercent,
                        aggregate.MaxDownPercent.PriceRange,
                        aggregate.MaxDownPercent.Direction,
                        aggregate.MaxDownPercent.AverageDailyGainInDollar,
                        aggregate.DowndayAverageGain
                        );
                    Console.WriteLine("Total Change Hand Percentage {0:p}", aggregate.TotalChangeHandPercentage);

                    PriceDataPoint prev = null;
                    foreach (var point in aggregate.MaxDrawDowns(5))
                    {
                        if (prev != null )
                            Console.WriteLine("after {0} trading days", point.Index - prev.Index);
                        Console.WriteLine("Big drawdown {0:p} on {1} {2}", point.Change, point.Timestamp.ToShortDateString(), point.Timestamp.DayOfWeek);
                        prev = point;
                        
                    }

                    foreach (var point in aggregate.MaxJumpups(5))
                    {
                        if (prev != null)
                            Console.WriteLine("after {0} trading days", point.Index - prev.Index);
                        Console.WriteLine("Big jump {0:p} on {1} {2}", point.Change, point.Timestamp.ToShortDateString(), point.Timestamp.DayOfWeek);
                        prev = point;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(aggregate.Ticker + " " + ex.Message);
                }
            }
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
            var setting = new YahooQuotesSetting();
            setting.Properties = new QuoteProperty[] {
                QuoteProperty.LastTradePriceOnly,
                QuoteProperty.ChangeInPercent,
                QuoteProperty.OneyrTargetPrice,
                QuoteProperty.PercentChangeFromFiftydayMovingAverage,
                QuoteProperty.PercentChangeFromTwoHundreddayMovingAverage            
            };

            setting.IDs = new string[] { "^TNX" };//MyHelper.GetStringToken(tickers, new string[] { ";", "," });
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
        public async Task AnalystRatingsDownloaderTest()
        {
            var setting = new AnalystRatingsSetting("AAPL");
            var downloader = new AnalystRatingsDownloader(setting);
            //var result = await downloader.PostDownload(
            //    new Dictionary<string,string>{{"textSearch", "JD" }});
            var result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            foreach (var item in result.AnalystRatings)
                PrintProperties(item, 0);
        }

        [TestMethod]
        public async Task CpiDownloaderTest ()
        {
            var setting = new CpiDataSetting();
            var downloader = new CpiDataDownloader(setting);
            var result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);

            foreach (var item in result.Items)
                PrintProperties(item, 0);
        }

        [TestMethod]
        public async Task TrefisDownloaderTest ()
        {
            var setting = new TrefisSetting();
            var downloader = new TrefisDownloader(setting);
            TrefisCompanyCoveredInfoAggregate result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
            var bib = result["bib"];
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

        [TestMethod]
        public async Task GetNasdaqHoldingDownloadTest()
        {
            var setting = new NasdaqHoldingSetting("SLXP");
            var downloader = new NasdaqHoldingDownloader(setting);
            var result = await downloader.PostDownload( 
                new Dictionary<string,string>{{"Command","marketvalue" }, {"sortorder", "1"}, {"page","1"}});
         //   var result = await downloader.DownloadObjectTaskAsync().ConfigureAwait(false);
         //   var result2 = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
            PrintProperties(result, 0);
         //   PrintProperties(result2, 0);
        }

        public static SyndicationFeed AtomFeed(string uri)
        {
            if (!string.IsNullOrEmpty(uri))
            {
                var ff = new Atom10FeedFormatter(); // Rss20FeedFormatter for Atom you can use Atom10FeedFormatter()
                var xr = XmlReader.Create(uri);
                ff.ReadFrom(xr);
                return ff.Feed;
            }
            return null;
        }


        [TestMethod]
        public async Task GetSecDownloadTest()
        {
            string bakerBrotherRSS = "http://www.sec.gov/cgi-bin/browse-edgar?action=getcompany&CIK=0001263508&type=4%25&dateb=&owner=include&start=0&count=100&output=atom";
            var setting = new SecEdgarSetting();
            var downloader = new SecEdgarDownloader(setting);            

                 var feed = AtomFeed(bakerBrotherRSS);
                 var sb = new StringBuilder(200);
            foreach (var link in feed.Items)
            {
                var linkUrl = link.Links[0].Uri.AbsoluteUri;
                var result = await downloader.DownloadObjectTaskAsync(linkUrl).ConfigureAwait(false);
                sb.AppendLine(result.ToString());
            }

            //MyHelper.SendEmail("Institution Baker Brother accumulation Action ", sb.ToString(), friendsEmail);
        }

        [TestMethod]
        public async Task CalculateFairValue()
        {
          //  var tenYearsBondYield = await DownloadHelper.GetQuote(new string[] { "^TNX" }, QuoteProperty.LastTradePriceOnly, QuoteProperty.ChangeInPercent);
          //  var currentPrice = DownloadHelper.GetQuote(new string[] { ticker }, QuoteProperty.LastTradePriceOnly, QuoteProperty.ChangeInPercent);

            double inflation = await DownloadHelper.GetCpiData(); //todo: get from yahoo quote
            double fixincomeReturnRate = 0.0796; //todo: get from yahoo quote
            var setting = new YahooCompanyStatisticsSetting("CSCO");
            var dl = new YahooCompanyStatisticsDownloader(setting);
            //var bag = new ConcurrentBag<CompanyStatisticsData>();
            var results = await dl.BatchDownloadObjectsTaskAsync(setting.GetUrls(tickers)).ConfigureAwait(false);

            //Parallel.ForEach(tickers, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
            //    id =>
            //    {
            //        var resp = dl.BatchDownloadObjectsTaskAsync(id);
            //        if (resp.Result.Connection.State == ConnectionState.Success)
            //        {

            //            var item = resp.Result.Result.Item;
            //            bag.Add(item);
            //        }
            //    });
            results.OrderBy(result => { return result.Item.TradingInfo.TwoHundredDayMovingAverage; });
            //TODO: Add property FairValue and Market Price in CompanyStatisticsData
            foreach (var result in results)
            {
                var ti = result.Item.TradingInfo;
                var vm = result.Item.ValuationMeasures;
                var highlight = result.Item.FinancialHighlights;
                var eps = result.Item.FinancialHighlights.DilutedEPS;

                // var growthRate1 = vm.TrailingPE / vm.ForwardPE;
                var growthRate = highlight.QuarterlyRevenueGrowthPercent;//.QuaterlyEarningsGrowthPercent /100.0;
                //var outStandingShare = vm.MarketCapitalisationInMillion / highlight.RevenuePerShare.

                var fairValue = FairValueEngine.DiscountedCurrentValue(eps, 3, growthRate / 100.0, inflation/10000.0, fixincomeReturnRate);
                if (eps <= 0 && fairValue <= 0)
                    fairValue = FairValueEngine.FutureValue(highlight.RevenuePerShare, growthRate / 100.0, 1) * 1.5;
                Debug.WriteLine("{0}      FairValue : {1}      forward P/E : {2}       EV/Rev : {3}       Margin: {4}          ShortPercentage : {5}       EPS: {6}      GrowthRate: {7}",
                    result.Item.ID.PadRight(5),
                    fairValue.ToString("C").PadLeft(9),
                    vm.ForwardPE.ToString().PadLeft(8),
                    vm.EnterpriseValueToRevenue.ToString().PadLeft(8),
                    (highlight.ProfitMarginPercent / 100.0).ToString("P").PadLeft(9),
                    (ti.ShortPercentOfFloat / 100.0).ToString("P").PadLeft(8),
                    eps.ToString("C").PadLeft(8),
                    (growthRate / 100.0).ToString("P").PadLeft(8));
            }

        }

        
        public void SendSMS (string msg)
        {
          
        /*    var msg = @"亲爱的朋友, 您将自动成为Adam开发的交易系统的首批用户通知子系统
                 该系统现在主要研究是分析明牌，从重要网站数据采集，不犯方向性错误。主要关注四种策略，好骑师，灰马，BUYOUT和庄家自救，4种占齐胜算是95%以上，
                 系统主要是避免人性的弱点和心情的起伏，不会犯低级错误，
                 后面阶段将用NEURAL NETWORK建立预测系统，定价，趋势的提前预见，ORDER MANAGEMENT，机会寻找，胜算估计，自动交易。
                 BACKEND可能会用WEB API， 前台可能会是SPA";*/
          
             //   MyHelper.SendTextMessage("慢点开车", "想", 7184042681, CarrierGateWay.TMOBILE);
             //   MyHelper.SendTextMessage("Test", "Hello, 你受到这封信，是因为你认识Adam，开发一个交易系统的通知子系统", 9172079948, CarrierGateWay.ATT);
                MyHelper.SendTextMessage("订阅服务 : ",msg, CarrierGateWay.TMOBILE,friendTmus);
          

           //   MyHelper.SendTextMessage("慢点开车", "想", 7184042681, CarrierGateWay.TMOBILE);
                //   MyHelper.SendTextMessage("Test", "Hello, 你受到这封信，是因为你认识Adam，开发一个交易系统的通知子系统", 9172079948, CarrierGateWay.ATT);
                MyHelper.SendTextMessage("订阅服务 : ", msg, CarrierGateWay.ATT, friendAtt);
           
        }
        
        [TestMethod]
        public void GetCalendars()
        {
            Calendar[] myOptCals = new CultureInfo("en-US").OptionalCalendars;
            var Calendar = new GregorianCalendar(GregorianCalendarTypes.USEnglish);
            // Checks which ones are GregorianCalendar then determines the GregorianCalendar version.
            Debug.WriteLine("The en-US culture supports the following calendars:");
            foreach (Calendar cal in myOptCals)
            {
                if (cal.GetType() == typeof(GregorianCalendar))
                {
                    GregorianCalendar myGreCal = (GregorianCalendar)cal;
                    GregorianCalendarTypes calType = myGreCal.CalendarType;
                    Debug.WriteLine("   {0} ({1})", cal, calType);
                }
                else
                {
                    Debug.WriteLine("   {0} {1}", cal, cal.AlgorithmType);
                }
            }
        }

        [TestMethod]
        public async Task GetNasdaqEarningForcast()
        {
            var setting = new NasdaqEarningForecastSetting("LNG");
            var downloader = new NasdaqEarningForecastDownloader(setting);
            var result = await downloader.BatchDownloadObjectsStreamTaskAsync(setting.GetUrls(tickers)).ConfigureAwait(false);
         //   var item = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
            //foreach (var q in result.QuarterlyEarningForecasts)
            //{
            //    Console.WriteLine(q..ConsensusEpsForecast)
            //}
            //var serializer = new JavaScriptSerializer();
            //String json = serializer.Serialize(result);
            //Debug.WriteLine(json);
            //var column1 = "Ticker"; var col1With = column1.Length + 2;
            //var column2 = "Q2Q Growth"; var col2With = column2.Length + 8;
            //var column3 = "Y2Y Growth"; var col3With = column3.Length + 8;
            //Console.Write(column1.PadRight(col1With, '.'));
            //Console.Write(column2.PadLeft(col2With, '.'));
            //Console.WriteLine(column3.PadLeft(col3With, '.'));
            var orderResult = result.OrderByDescending(item => { if (item.YearlyEarningForecasts.Length == 0) return double.NaN; return item.YearlyEarningForecasts.Last().ConsensusEpsForecast; });
            foreach (var item in orderResult)
            {
                try
                {
                    Console.WriteLine(item.ToString());
                    //Console.Write(item.Ticker.PadRight(col1With, '.'));
                    //Console.Write(item.QuartylyEarningGrowth.ToString("p").PadLeft(col2With,'.'));
                    //Console.WriteLine(item.QuartylyEarningGrowth.ToString("p").PadLeft(col3With, '.'));
                //{
                //    Console.WriteLine("ticker: {0}          q2q growth: {1}                    y2y growth: {2}", 
                //        item.Ticker.PadRight(6), 
                //        item.QuartylyEarningGrowth.ToString("p").PadLeft(25), 
                //        item.YearlyEarningGrowth.ToString("p").PadLeft(25));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(item.Ticker + "  " + ex.Message);
                }
                 //foreach (var y in item.YearlyEarningForecasts)
                 //{
                 //    PrintProperties(y, 0);
                 //}

                 //foreach (var q in item.QuarterlyEarningForecasts)
                 //{
                 //    PrintProperties(q, 0);
                 //}
            }
        }

        [TestMethod]
        public async Task GetSPYValuationHistoryMetrics()
        {
            var setting = new SPYValuationSetting("SPY");
            var downloader = new SPYValuationDownloader(setting);
            var result = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
            foreach (var item in result.Items)
            {

                Console.WriteLine("ticker: SPY - {0} Date: {1} Value: {2}", result.MetricsType.ToString(), item.Date, item.Value);
            }
        }

        [TestMethod]
        public async Task GetMorningStarValuationHistoryMetrics()
        {
            var setting = new MorningStarValuationSetting("RCL");
            var downloader = new MorningStarValuationDownloader(setting);
            var curVal = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);

            setting.IsForwardValuation = true;
            var forwardVal = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
            PrintProperties(curVal, 0);

            PrintProperties(forwardVal, 0);
        }

        [TestMethod]
        public async Task GetNasdaqEarningHistory()
        {
            var setting = new NasdaqEarningHistorySetting("PCYC");
            var downloader = new NasdaqEarningHistoryDownloader(setting);
            var result = await downloader.BatchDownloadObjectsStreamTaskAsync(setting.GetUrls("PCYC,INCY,GEVA,ACAD")).ConfigureAwait(false);
            // var result = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
            //foreach (var q in result.QuarterlyEarningForecasts)
            //{
            //    Console.WriteLine(q..ConsensusEpsForecast)
            //}
            var serializer = new JavaScriptSerializer();
            String json = serializer.Serialize(result);
            Debug.WriteLine(json);
            foreach (var item in result)
            {
           //      foreach (var q in result.Items)//item.Items)
                {
                   // PrintProperties(q, 0);
                    Console.WriteLine("ticker: {0} growth: {1}", item.Ticker, item.QuartylyEarningGrowth);
                }
            }
        }

        [TestMethod]
        public async Task SpyDataRepositoryTest()
        {
            var setting = new SPYValuationSetting("SPY");
            var downloader = new SPYValuationDownloader(setting);
            var result = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);

            setting.Valuationtype = ValuationType.PB;
            var result2 = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
            var repo = new SpyDataRepository();
            repo.Save(@"c:\temp\spy.xml",result, result2);

            var result_ = repo.Load(@"c:\temp\spy.xml");
            //var xmlSerializer = new XmlSerializer(typeof(MetricsDataPointResult));
            //using (var reader = XDocument.CreateReader())
            //{
            //    var val = (MetricsDataPointResult)xmlSerializer.Deserialize(reader);
            //}             
        }

        [TestMethod]
        public async Task YahooValuationPointTest()
        {
            var setting = new YahooValuationSetting();
            var downloader = new YahooValuationDownloader(setting);
            var result = await downloader.DownloadObjectStreamTaskAsync(setting.GetUrl("AAPL")).ConfigureAwait(false);
            PrintProperties(result.Sector, 0);
            PrintProperties(result.Industry, 0);
            PrintProperties(result.Self, 0);

        }

        [TestMethod]
        public async Task MorningStarPerformanceTest()
        {
            var setting = new MorningStarPerformanceSetting("MSFT");
            var downloader = new MorningStarPerformanceDownloader(setting);
            var result = await downloader.DownloadObjectStreamTaskAsync().ConfigureAwait(false);
            PrintProperties(result.StockPerformance, 0);
            PrintProperties(result.Industryformance, 0);
            PrintProperties(result.SP500formance, 0);
        }

        [TestMethod]
        public async Task StockContextTest()
        {
            var symbles = MyHelper.GetStringToken(tickers, new string[] { ";", "," });////new string[] {"AAPL", "CSCO", "ACAD", "SGEN", "GOOGL"};
            var context = new StockContext(symbles);

            var sector = context.SectorByTicker("AAPL");
            var industry = context.IndustryByTicker("AAPL");
            var equity = context.EquityByTicker("AAPL");

            var fiveAvgPE = context.CurrentMorningStarValuationMetric.FiveYearsAvgPE;
            var currentPE = context.CurrentMorningStarValuationMetric.CurrentPE;
            var forwardPE = context.ForwardMorningStarValuationMetric.ForwardPE;
            var growth = currentPE / forwardPE - 1;
            var bondRate = await DownloadHelper.GetRiskFreeRate();

        }

        public void PrintProperties(object obj, int indent)
        {
            if (obj == null) return;
            string indentString = new string(' ', indent);
            Type objType = obj.GetType();
            PropertyInfo[] properties = objType.GetProperties(
                                    BindingFlags.Public | 
                                    BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                if (property.GetIndexParameters().Length>0)
                    continue;

                object propValue = property.GetValue(obj, null);
                if (property.PropertyType.Assembly == objType.Assembly)
                {
                    if(property.PropertyType.IsEnum)
                        Console.WriteLine("{0}{1}: {2}", indentString, property.Name, propValue);
                    else
                    {
                        Console.WriteLine("{0}{1}:", indentString, property.Name);
                        PrintProperties(propValue, indent + 2);
                    }
                }
                else
                {
                    Console.WriteLine("{0}{1}: {2}", indentString, property.Name, propValue);
                }
            }
        }
        protected IEnumerable<Task> GetScheduledTasks()
        {
            yield break;
        }
    }
}
